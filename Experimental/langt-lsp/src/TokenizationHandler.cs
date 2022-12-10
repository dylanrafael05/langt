using Langt.AST;
using Langt.Codegen;
using Langt.Lexing;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace Langt.LSP;

public class TokenizationHandler : DocumentSymbolHandlerBase
{
    public override Task<SymbolInformationOrDocumentSymbolContainer> Handle(DocumentSymbolParams request, CancellationToken cancellationToken)
    {
        var result = new List<SymbolInformationOrDocumentSymbol>();

        void Add() 
        {
            var ds = new SymbolInformation(); // TODO: use document symbol
            ds.
            result.Add(new SymbolInformationOrDocumentSymbol(new DocumentSymbol(ds)));
        }

        var proj = new LangtProject();

        var path = request.TextDocument.Uri.GetFileSystemPath();

        proj.AddFileContents(path, File.ReadAllText(path));

        var astNode = proj.ParsedFiles[path];
        
        var categorizer = new SourceCategorizer();
        // TODO; split out to a distinct function!
        void PassCategorizer(ASTNode x, SourceCategorizer categorizer, Func<TokenCategory, TokenCategory>? passAlong = null)
        {
            if(x is ASTType t)
            {
                passAlong = c => c is TokenCategory.Identifier ? TokenCategory.Type : c;
            }

            if(x is FunctionCall f)
            {
                PassCategorizer(f.Function, categorizer, c => c is TokenCategory.Identifier ? TokenCategory.Function : c);
                foreach(var c in f.Children.Where(c => c != f.Function))
                {
                    PassCategorizer(c, categorizer, passAlong);
                }
                return;
            }

            if(x is ASTToken tok) 
            {
                categorizer.Categorize(tok.Range, passAlong?.Invoke(tok.Inner.Category) ?? tok.Inner.Category);
            }

            foreach(var c in x.AllChildren)
            {
                PassCategorizer(c, categorizer, passAlong);
            }
        }
        PassCategorizer(astNode, categorizer);

        foreach(var c in categorizer.Categorizations) 
        {
            result.Add(new()
                c.Range.LineStart,
                c.Range.CharStart,
                c.Range.CharEnd - c.Range.CharStart,
                c.Category switch 
                {
                    TokenCategory.Identifier => SemanticTokenType.Variable,
                    TokenCategory.Function   => SemanticTokenType.Function,
                    TokenCategory.Type       => SemanticTokenType.Type,

                    TokenCategory.Keyword => SemanticTokenType.Keyword,

                    TokenCategory.String => SemanticTokenType.String,
                    TokenCategory.NumericLiteral => SemanticTokenType.Number,
                    TokenCategory.Brace or TokenCategory.Operator => SemanticTokenType.Operator,

                    _ => (SemanticTokenType?)null
                }
            );
        }

        builder.Commit();

        return Task.CompletedTask;
    }

    protected override DocumentSymbolRegistrationOptions CreateRegistrationOptions(DocumentSymbolCapability capability, ClientCapabilities clientCapabilities)
    {
        throw new NotImplementedException();
    }

    protected override Task<SemanticTokensDocument> GetSemanticTokensDocument(ITextDocumentIdentifierParams @params, CancellationToken cancellationToken)
    {
    }
}