using Langt.Structure;

namespace Langt.CG.Bindings;

public class ConversionBuilder : Builder<LangtConversion, CodeGenerator.Applicator>
{
    public override CodeGenerator.Applicator Build(LangtConversion cv)
    {
        if(cv.ID == ConversionID.Deference  ) return BuildDereference(cv);
        if(cv.ID == ConversionID.AliasConst ) return BuildAliasConst (cv);
        if(cv.ID == ConversionID.AliasDest  ) return BuildAliasDest  (cv);
        if(cv.ID == ConversionID.PointerCast) return BuildPointerCast(cv);
        if(cv.ID == ConversionID.OptionConst) return BuildOptionConst(cv);

        return BuildOther(cv);
    }

    public CodeGenerator.Applicator BuildDereference(LangtConversion cv) 
        => l => CG.Builder.BuildLoad2(CG.Binder.Get(cv.Output), l);
    
    public CodeGenerator.Applicator BuildAliasConst(LangtConversion _) 
        => l => l;
    public CodeGenerator.Applicator BuildAliasDest(LangtConversion _) 
        => l => l;
    public CodeGenerator.Applicator BuildPointerCast(LangtConversion _) 
        => l => l;

    public CodeGenerator.Applicator BuildOptionConst(LangtConversion cv) 
        => l => 
        {
            var s = CG.Builder.BuildAlloca(CG.Binder.Get(cv.Output));
            CG.Builder.BuildStore(l, s);
            
            var t = CG.Builder.BuildStructGEP2(CG.Binder.Get(cv.Output), s, LangtOptionType.TagLocation);
            CG.Builder.BuildStore(LLVMValueRef.CreateConstInt(LLVMTypeRef.Int8, (ulong)cv.Output.OptionTypeMap![cv.Input]), t);

            return CG.Builder.BuildLoad2(CG.Binder.Get(cv.Output), s);
        };

    public CodeGenerator.Applicator BuildOther(LangtConversion cv)
    {
        if(cv.Input.IsInteger && cv.Output.IsInteger)
        {
            return l => CG.Builder.BuildIntCast(l, CG.Binder.Get(cv.Output));
        }
        else if(cv.Input.IsReal && cv.Output.IsReal)
        {
            return l => CG.Builder.BuildFPCast(l, CG.Binder.Get(cv.Output));
        }
        else if(cv.Input.IsInteger && cv.Output.IsReal)
        {
            return l => CG.Builder.BuildCast(LLVMOpcode.LLVMSIToFP, l, CG.Binder.Get(cv.Output));
        }
        else if(cv.Input.IsReal && cv.Output.IsInteger)
        {
            return l => CG.Builder.BuildCast(LLVMOpcode.LLVMFPToSI, l, CG.Binder.Get(cv.Output));
        }

        throw new NotSupportedException($"Unknown conversion {cv.ID.Name}");
    }
}