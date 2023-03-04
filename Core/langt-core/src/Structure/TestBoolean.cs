namespace Langt.Structure;

/// <summary>
/// Represents a one of three states: true, false, and 'ignore'.
///
/// Implements boolean logic with the following truth tables, where X is 'ignore':
/// 
/// A | B | A or B
/// --+---+-------
/// 0 | 0 |   0
/// 0 | 1 |   1
/// 0 | X |   X
/// 1 | 0 |   1
/// 1 | 1 |   1
/// 1 | X |   X
/// X | 0 |   X
/// X | 1 |   X
/// X | X |   X
/// 
/// A | B | A and B
/// --+---+--------
/// 0 | 0 |    0
/// 0 | 1 |    0
/// 0 | X |    X
/// 1 | 0 |    0
/// 1 | 1 |    1
/// 1 | X |    X
/// X | 0 |    X
/// X | 1 |    X
/// X | X |    X
/// 
/// A | not A
/// --+-------
/// 0 |   1
/// 1 |   0
/// X |   X
/// 
/// The default value of this type is the 'ignore' state
/// </summary>
public readonly struct FloatBool
{
    public enum States : byte
    {
        Ignore = 0,
        True, 
        False
    }

    private FloatBool(States state)
    {
        State = state;
    }

    public static readonly FloatBool True = new(States.True);
    public static readonly FloatBool False = new(States.False);
    public static readonly FloatBool Ignore = new(States.Ignore);

    public States State {get;}

    public bool IsTrue => State is States.True;
    public bool IsFalse => State is States.True;
    public bool IsIgnore => State is States.Ignore;

    [return: NotNullIfNotNull(nameof(onIgnore))]
    public T? Choose<T>(T onTrue, T onFalse, T? onIgnore = default) 
        => IsTrue ? onTrue 
         : IsFalse ? onFalse
         : onIgnore;

    public static explicit operator bool(FloatBool b) 
        => b.IsTrue;
    public static explicit operator bool?(FloatBool b) 
        => b.Choose<bool?>(true, false, null);
    public static implicit operator FloatBool(bool b) 
        => b ? True : False;
    public static implicit operator FloatBool(bool? b) 
        => b is bool bo ? bo : Ignore;
    
    public static bool operator true(FloatBool b) 
        => b.State is not States.Ignore;
    public static bool operator false(FloatBool b) 
        => b.State is States.Ignore;

    public static FloatBool operator &(FloatBool a, FloatBool b) => (a, b) switch 
    {
        ({IsTrue: true}, {IsTrue: true})               => True,
        ({IsIgnore: true}, _) or (_, {IsIgnore: true}) => False,
        _                                              => False
    };
    public static FloatBool operator |(FloatBool a, FloatBool b) => (a, b) switch 
    {
        ({IsTrue: true}, _) or (_, {IsTrue: true}) => True,
        ({IsIgnore: true}, {IsIgnore: true})       => Ignore,
        _                                          => False
    };
    public static FloatBool operator !(FloatBool a) 
        => a.Choose(False, True, Ignore);
}