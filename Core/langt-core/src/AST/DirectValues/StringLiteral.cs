using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;
using Langt.Utility;

namespace Langt.AST;

public record StringLiteral(ASTToken Tok) : ASTNode, IDirectValue
{
    public override ASTChildContainer ChildContainer => new() {Tok};

    public override void Dump(VisitDumper visitor)
        => visitor.VisitNoDepth(Tok);

    public string? Value {get; private set;}

    public override void TypeCheckRaw(CodeGenerator generator)
    {
        var source = Tok.Content;
        var s = "";
        
        for(int i = 1; i < source.Length - 1; i++)
        {
            var c = source[i];

            if(c is not '\\')
            {
                s += c;
            }
            else
            {
                s += source[i+1] switch 
                {
                    'n' => '\n',
                    'r' => '\r',
                    't' => '\t',

                    '0' => '0',

                    '"' => '"',
                    '\\' => '\\',

                    var u => Functional.Do(() => generator.Diagnostics.Error($"Unrecognized string escape sequence '\\{u}'", Range), '\0')
                };
                i++;
            }
        }

        Value = s;

        RawExpressionType = LangtType.PointerTo(LangtType.Int8);
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        var res = lowerer.Builder.BuildGlobalStringPtr(Value!, "str");

        lowerer.PushValue( 
            RawExpressionType,
            res,
            DebugSourceName
        );
    }
}