using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;
using Langt.Utility;
using Langt.Utility.Functional;

namespace Langt.AST;

public record BoundStringLiteral(StringLiteral Source, string Value) : BoundASTNode(Source) 
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {};

    public override void LowerSelf(CodeGenerator generator)
    {
        var res = generator.Builder.BuildGlobalStringPtr(Value, "str");

        generator.PushValue( 
            RawExpressionType,
            res,
            DebugSourceName
        );
    }
}

public record StringLiteral(ASTToken Tok) : ASTNode, IDirectValue
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Tok};

    public override void Dump(VisitDumper visitor)
        => visitor.VisitNoDepth(Tok);

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
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
                (char nc, bool err) = source[i+1] switch 
                {
                    'n' => ('\n', false),
                    'r' => ('\r', false),
                    't' => ('\t', false),

                    '0' => ('\0', false),

                    '\"' => ('\"', false),
                    '\\' => ('\\', false),

                    _ => ('\0', true)
                };

                if(!err)
                {
                    return ResultBuilder.Empty().WithDgnError($"Unrecognized string escape sequence '\\{source[i+1]}'", Range).Build<BoundASTNode>();
                }

                s += nc;

                i++;
            }
        }

        return Result.Success<BoundASTNode>
        (
            new BoundStringLiteral(this, s) 
            {
                RawExpressionType = LangtType.PointerTo(LangtType.Int8)
            }
        );
    }
}