using Langt.Structure;


namespace Langt.CG;

public static class NameMangling
{
    public const string Prepend = "_L";

    public static string MangledName(this LangtFunction fn) 
    {
        var n = Prepend;

        n += MangledScope(fn.Group.HoldingScope) + fn.Name.Length + fn.Name;

        n += fn.Type.ParameterTypes.Length;
        foreach(var p in fn.Type.ParameterTypes)
        {
            n += p.PreMangledName();
        }

        return n;
    }

    public static string MangledName(this LangtType ty) 
        => Prepend + PreMangledName(ty);

    public static string PreMangledName(this LangtType ty) 
    {
        var n = "";

        if(ty is LangtConsGenericStructureType cs) 
        {
            n = n + "G" + MangledScope(ty.HoldingScope) + cs.BaseType.Name.Length + cs.BaseType.Name;
            n += cs.ConstructionArguments.Count + "X";

            foreach(var c in cs.ConstructionArguments)
            {
                var s = PreMangledName(c);
                n += s.Length + s;
            }

            return n;
        }
        else if(ty.IsOption)
        {
            n = n + "O" + MangledScope(ty.HoldingScope) + ty.Name.Length + ty.Name;
            n += ty.OptionTypes.Count + "X";

            foreach(var c in ty.OptionTypes)
            {
                var s = PreMangledName(c);
                n += s.Length + s;
            }

            return n;
        }
        else if(ty.IsPointer)
        {
            return "P" + PreMangledName(ty.ElementType);
        }
        else if(ty.IsReference)
        {
            return "R" + PreMangledName(ty.ElementType);
        }
        else 
        {
            return n + "S" + MangledScope(ty.HoldingScope) + ty.Name.Length + ty.Name;
        }
    }
    
    private static string MangledScope(IScope? sc) 
    {
        var stack = new Stack<string>();

        while(sc is not null)
        {
            if(sc is IFullNamed nsc)
                stack.Push(nsc.Name.Length + nsc.Name);
            
            sc = sc.Parent;
        }

        var k = stack.Count + 1 + "X";

        while(stack.Count > 0)
        {
            k += stack.Pop();
        }

        return k;
    }
}
