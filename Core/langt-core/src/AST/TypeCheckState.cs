using System.Diagnostics.CodeAnalysis;
using Langt.Codegen;

namespace Langt.AST;

// TODO: is it safe to allow for "no fail" execution of passes?
// TODO: in what circumstances would "no fail" execution be requried?
// It appears that the main case for this is resolution; is there another way to report resolution failures?
public abstract record ASTPassState(CodeGenerator CG, bool Noisy, bool CanFail)
{
    public abstract ASTPassException ProduceException();

    public void Error(string message, SourceRange range)
    {
        if(Noisy)
        {
            CG.Diagnostics.Error(message, range);
        }

        if(CanFail)
        {
            throw ProduceException();
        }
    }
    public void Warning(string message, SourceRange range)
    {
        if(Noisy)
        {
            CG.Diagnostics.Warning(message, range);
        }
    }
    public void Note(string message, SourceRange range)
    {
        if(Noisy)
        {
            CG.Diagnostics.Note(message, range);
        }
    }
}

public abstract record ASTPassState<TException>(CodeGenerator CG, bool Noisy, bool CanFail) : ASTPassState(CG, Noisy, CanFail)
    where TException : ASTPassException, new()
{
    public override ASTPassException ProduceException() => new TException();
}

[Serializable]
public class ASTPassException : Exception
{
    public ASTPassException() {}
}

public record GeneralPassState(CodeGenerator CG, bool Noisy, bool CanFail) : ASTPassState<ASTPassException>(CG, Noisy, CanFail)
{
    public static GeneralPassState Start(CodeGenerator gen) => new(gen, true, true);
}

public record TypeCheckState(CodeGenerator CG, bool TryRead, bool Noisy, bool CanFail) : ASTPassState<TypeCheckException>(CG, Noisy, CanFail)
{
    public static TypeCheckState Start(CodeGenerator gen) => new(gen, true, true, true);

    private bool MatchInternal(LangtType to, ASTNode from, out ASTTypeMatchCreator matcher) 
    {
        matcher = new();
        
        if(from.RequiresTypeDownflow)
        {
            if(!from.FinalizedTypeChecking && !from.TryTypeCheck(this with {Noisy = false}, to)) 
            {
                return false;
            }

            matcher = matcher with {DownflowType = to};
        }
        
        var ftype = from.TransformedType;

        if(to == ftype) return true;

        if(from.IsLValue && ftype.PointeeType == to)
        {
            matcher = matcher with {Transformer = LangtReadPointer.Transformer(ftype)};
            return true;
        }

        var conv = CG.ResolveConversion(to, ftype);
        if(conv is null) return false;

        matcher = matcher with {Transformer = conv.TransformProvider.TransformerFor(ftype, to)};
        return conv.IsImplicit;
    }

    public bool CanMatch(LangtType to, ASTNode from, out ASTTypeMatchCreator matcher)
        => MatchInternal(to, from, out matcher);
    public bool MakeMatch(LangtType to, ASTNode from) 
    {
        if(!MatchInternal(to, from, out var matcher))
        {
            return false;
        }
        
        matcher.ApplyTo(from, this);
        return true;
    }
}
