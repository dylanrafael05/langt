using Langt.Lexing;
using Langt.Message;
using Langt.Structure;
using Langt.Structure.Visitors;
using Langt.Utility;

namespace Langt.AST;

public record BoundStringLiteral(StringLiteral Source, string Value) : BoundASTNode(Source) 
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {};
}

public record StringLiteral(ASTToken Tok) : ASTNode, IDirectValue
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Tok};

    protected override Result<BoundASTNode> BindSelf(Context ctx, TypeCheckOptions options)
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

                if(err)
                {
                    return ResultBuilder.Empty().WithDgnError(Messages.Get("escape-char", source[i+1]), Range).BuildError<BoundASTNode>();
                }

                s += nc;

                i++;
            }
        }

        return Result.Success<BoundASTNode>
        (
            new BoundStringLiteral(this, s) 
            {
                Type = new LangtPointerType(LangtType.UInt8)
            }
        );
    }
}