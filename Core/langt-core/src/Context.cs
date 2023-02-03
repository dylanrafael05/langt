using Langt.AST;
using Langt.Structure;
using Langt.Lexing;
using Langt.Parsing;
using Langt.Structure.Visitors;
using Langt.Structure.Resolutions;

namespace Langt;

public class Context : IProjectDependency
{
    public static readonly string[] AllMagicNames = 
    {
        LangtWords.MagicAdd,
        LangtWords.MagicSub,
        LangtWords.MagicMul,
        LangtWords.MagicDiv,
        LangtWords.MagicMod,
        
        LangtWords.MagicEq,
        LangtWords.MagicNotEq,
        LangtWords.MagicLess,
        LangtWords.MagicLessEq,
        LangtWords.MagicGreat,
        LangtWords.MagicGreatEq,
        
        LangtWords.MagicNeg,
        LangtWords.MagicNot,
        
        LangtWords.MagicIndex,
        LangtWords.MagicSetIndex,
    };

    public static Dictionary<OperatorSpec, string> MagicNames {get;} = new() 
    {
        [new(OperatorType.Binary, TokenType.Plus        )] = LangtWords.MagicAdd,
        [new(OperatorType.Binary, TokenType.Minus       )] = LangtWords.MagicSub,
        [new(OperatorType.Binary, TokenType.Star        )] = LangtWords.MagicMul,
        [new(OperatorType.Binary, TokenType.Slash       )] = LangtWords.MagicDiv,
        [new(OperatorType.Binary, TokenType.Percent     )] = LangtWords.MagicMod,
        
        [new(OperatorType.Binary, TokenType.DoubleEquals)] = LangtWords.MagicEq,
        [new(OperatorType.Binary, TokenType.NotEquals   )] = LangtWords.MagicNotEq,
        [new(OperatorType.Binary, TokenType.LessThan    )] = LangtWords.MagicLess,
        [new(OperatorType.Binary, TokenType.LessEqual   )] = LangtWords.MagicLessEq,
        [new(OperatorType.Binary, TokenType.GreaterThan )] = LangtWords.MagicGreat,
        [new(OperatorType.Binary, TokenType.GreaterEqual)] = LangtWords.MagicGreatEq,

        [new(OperatorType.Unary , TokenType.Minus       )] = LangtWords.MagicNeg,
        [new(OperatorType.Unary , TokenType.Not         )] = LangtWords.MagicNot,
    };
    public static Dictionary<string, string> MagicNameToDescription {get;} = new()
    {
        [LangtWords.MagicAdd    ] = "operator +",
        [LangtWords.MagicSub    ] = "operator -",
        [LangtWords.MagicMul    ] = "operator *",
        [LangtWords.MagicDiv    ] = "operator /",
        [LangtWords.MagicMod    ] = "operator %",

        [LangtWords.MagicEq     ] = "operator ==",
        [LangtWords.MagicNotEq  ] = "operator !=",
        [LangtWords.MagicLess   ] = "operator <" ,
        [LangtWords.MagicLessEq ] = "operator <=",
        [LangtWords.MagicGreat  ] = "operator >" ,
        [LangtWords.MagicGreatEq] = "operator >=",

        [LangtWords.MagicNeg    ] = "operator unary -",
        [LangtWords.MagicNot    ] = "operator not",
    };

    public static string DisplayableFunctionGroupName(string name) 
        => MagicNameToDescription.TryGetValue(name, out var value) ? value : name;

    public LangtProject Project {get; init;}

    public LangtFunction? CurrentFunction {get; set;}
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

    private readonly List<IConversionProvider> conversions = new();

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
        
        foreach(var conv in LangtConversion.Builtins)
        {
            DefineConversion(conv);
        }

        foreach(var op in AllMagicNames)
        {
            Project.GlobalScope.Define(s => new LangtFunctionGroup(op, s), SourceRange.Default).Expect();
        }

        BuiltinOperators.Initialize(this);
    }

    public LangtFunctionGroup GetGlobalFunction(string name)
    {
        return Project.GlobalScope.ResolveFunctionGroup(name, SourceRange.Default, false).Expect("Operators that are known must exist in the global scope.");
    }
    public LangtFunctionGroup GetOperator(OperatorSpec op)
    {
        if(!MagicNames.TryGetValue(op, out var name))
        {
            throw new InvalidOperationException($"Unknown operator passed to .{nameof(GetOperator)}");
        }

        return GetGlobalFunction(name);
    }

    public void DefineUnaryOperator(TokenType op, LangtType x, LangtType r)
    {
        var opfn = GetOperator(new(OperatorType.Unary, op));

        var ftype = LangtFunctionType.Create(new[] {x}, r).Expect();

        var fn = new LangtFunction(opfn)
        {
            Type           = ftype,
            ParameterNames = new[] {"__x"},
            IsExtern       = false
        };

        opfn.AddFunctionOverload(fn, SourceRange.Default).Expect("Cannot redefine operator");
    }

    public void DefineBinaryOperator(TokenType op, LangtType a, LangtType b, LangtType r)
    {
        var opfn = GetOperator(new(OperatorType.Binary, op));

        var ftype = LangtFunctionType.Create(new[] {a, b}, r).Expect();

        var fn = new LangtFunction(opfn)
        { 
            Type           = ftype,
            ParameterNames = new[] {"__a", "__b"},
            IsExtern       = false
        };

        opfn.AddFunctionOverload(fn, SourceRange.Default).Expect("Cannot redefine operator");
    }

    public void SetCurrentNamespace(LangtNamespace ns)
    {
        Expect.ArgNonNull(ns, "Cannot set file namespace to null!");

        CurrentFile!.RebaseScope(ns);

        ResolutionScope = CurrentFile.Scope;
        CurrentNamespace = ns;
    }

    public void DefineConversion(IConversionProvider conversion)
    {
        conversions.Add(conversion);
    }

    public IScope OpenScope()
        => ResolutionScope = new LangtScope(ResolutionScope);
    public void CloseScope()
    {
        ResolutionScope = ResolutionScope.HoldingScope ?? throw new Exception("Cannot close scope; the current scope is the global scope!");
    }

    public Result<LangtConversion> ResolveConversion(LangtType input, LangtType output, SourceRange range) 
    {
        var conv = conversions.Select(c => c.GetConversionFor(input, output)).FirstOrDefault(c => c.HasValue);

        if(conv is null)
        {
            return ResultBuilder.Empty()
                .WithDgnError($"Could not find a conversion from {input.FullName} to {output.FullName}", range)
                .BuildError<LangtConversion>()
            ;
        }
        
        return Result.Success(conv.Value);
    }
}