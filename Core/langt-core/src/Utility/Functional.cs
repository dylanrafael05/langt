namespace Langt.Utility;

public static class Functional 
{
    public static T Do<T>(Action action, T t) 
    {
        action();
        return t;
    }
}

