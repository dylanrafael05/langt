using Langt.AST;
using Langt.Structure;
using Langt.Lexing;
using Langt.Parsing;
using Langt.Structure.Visitors;
using Langt.Structure.Resolutions;

namespace Langt;

public class Context : IProjectDependency
{
    public static Dictionary<OperatorSpec, string> MagicNames {get;} = new() 
    {
        [new(OperatorType.Binary, TokenType.Plus        )] = "op_add"          ,
        [new(OperatorType.Binary, TokenType.Minus       )] = "op_sub"          ,
        [new(OperatorType.Binary, TokenType.Star        )] = "op_mul"          ,
        [new(OperatorType.Binary, TokenType.Slash       )] = "op_div"          ,
        [new(OperatorType.Binary, TokenType.Percent     )] = "op_mod"          ,
        
        [new(OperatorType.Binary, TokenType.DoubleEquals)] = "op_equal"        ,
        [new(OperatorType.Binary, TokenType.NotEquals   )] = "op_not_equal"    ,
        [new(OperatorType.Binary, TokenType.LessThan    )] = "op_less"         ,
        [new(OperatorType.Binary, TokenType.LessEqual   )] = "op_less_equal"   ,
        [new(OperatorType.Binary, TokenType.GreaterThan )] = "op_greater"      ,
        [new(OperatorType.Binary, TokenType.GreaterEqual)] = "op_greater_equal",

        [new(OperatorType.Unary , TokenType.Minus       )] = "op_neg"         ,
        [new(OperatorType.Unary , TokenType.Not         )] = "op_not"         ,
    };
    public static Dictionary<string, string> MagicNameToDescription {get;} = new()
    {
        ["op_add"          ] = "operator +",
        ["op_sub"          ] = "operator -",
        ["op_mul"          ] = "operator *",
        ["op_div"          ] = "operator /",
        ["op_mod"          ] = "operator %",

        ["op_equal"        ] = "operator ==",
        ["op_not_equal"    ] = "operator !=",
        ["op_less"         ] = "operator <" ,
        ["op_less_equal"   ] = "operator <=",
        ["op_greater"      ] = "operator >" ,
        ["op_greater_equal"] = "operator >=",

        ["op_neg"          ] = "operator unary -",
        ["op_not"          ] = "operator not",
    };

    public static string DisplayableFunctionGroupName(string name) 
        => MagicNameToDescription.TryGetValue(name, out var value) ? value : name;

    public LangtProject Project {get; init;}

    /// <summary>
    /// The scope in which resolution is currently taking place.
    /// </summary>
    public IScope ResolutionScope {get; private set;}
    /// <summary>
    /// The scope that the AST is currently inside of.
    /// </summary>
    /// <value></value>
    public IScope CurrentScope {get; private set;}
    /// <summary>
    /// Whether or not the resolution scope is the same as the current scope.
    /// </summary>
    public bool ResolutionScopeIsRedirected {get; private set;}
    /// <summary>
    /// The current file this code generator is working with.
    /// </summary>
    public LangtFile? CurrentFile {get; private set;}
    /// <summary>
    /// The current namespace as declared by 'namespace' directives.
    /// Will be null if in the global namespace or uninitialized/unrun.
    /// </summary>
    public LangtNamespace? CurrentNamespace {get; private set;}

    /// <summary>
    /// Open the given langt file with this code generator.
    /// </summary>
    /// <param name="file">The file to open.</param>
    public void Open(LangtFile file) 
    {
        CurrentFile = file;
        ResolutionScope = CurrentScope = file.Scope;
        CurrentNamespace = null;
    }

    private readonly List<LangtConversion> conversions = new();
    private readonly Dictionary<(LangtType to, LangtType from), LangtConversion> directConversions = new();

    /// <summary>
    /// The diagnostic collector used by the project this code generator depends on.
    /// </summary>
    public DiagnosticCollection Diagnostics => Project.Diagnostics;
    /// <summary>
    /// The logger used by the project this code generator depends on.
    /// </summary>
    public ILogger Logger => Project.Logger;

    public Context(LangtProject project)
    {
        Project = project;
        
        ResolutionScope = CurrentScope = project.GlobalScope;
        CurrentFile = null;

        Initialize();
    }

    public void Initialize()
    {
        // TYPES //
        foreach(var bt in LangtType.BuiltinTypes)
        { 
            Project.GlobalScope.DefineProxy(bt, SourceRange.Default).Expect();
        }
        
        foreach(var conv in LangtConversion.Builtin)
        {
            DefineConversion(conv);
        }

        foreach(var op in MagicNames.Values)
        {
            Project.GlobalScope.Define(s => new LangtFunctionGroup(op, s), SourceRange.Default).Expect();
        }

        BuiltinOperators.Initialize(this);
    }

    public LangtFunctionGroup GetOperator(OperatorSpec op)
    {
        if(!MagicNames.TryGetValue(op, out var name))
        {
            throw new InvalidOperationException($"Unknown operator passed to .{nameof(GetOperator)}");
        }

        return Project.GlobalScope.ResolveFunctionGroup(name, SourceRange.Default, false).Expect("Operators that are known must exist in the global scope.");
    }

    public void SetCurrentNamespace(LangtNamespace ns)
    {
        Expect.ArgNonNull(ns, "Cannot set file namespace to null!");

        CurrentFile!.RebaseScope(ns);

        ResolutionScope = CurrentFile.Scope;
        CurrentNamespace = ns;
    }

    public void DefineConversion(LangtConversion conversion)
    {
        if(conversion.TransformProvider.IsDirect)
        {
            directConversions.Add((conversion.TransformProvider.DirectResult!, conversion.TransformProvider.DirectInput!), conversion);
        }
        else
        {
            conversions.Add(conversion);
        }
    }

    public IScope OpenScope()
        => ResolutionScope = new LangtScope(ResolutionScope);
    public void CloseScope()
    {
        ResolutionScope = ResolutionScope.HoldingScope ?? throw new Exception("Cannot close scope; the current scope is the global scope!");
    }

    public Result<LangtConversion> ResolveConversion(LangtType to, LangtType from, SourceRange range) 
    {
        if(!directConversions.TryGetValue((to, from), out var conv))
        {
            conv = conversions.FirstOrDefault(c => c.TransformProvider.CanPerform(from, to));

            if(conv is null)
            {
                return ResultBuilder.Empty()
                    .WithDgnError($"Could not find a conversion from {from.FullName} to {to.FullName}", range)
                    .BuildError<LangtConversion>()
                ;
            }
        }
        
        return Result.Success(conv);
    }
}