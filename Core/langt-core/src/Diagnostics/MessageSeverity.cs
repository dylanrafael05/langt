using System.Diagnostics.CodeAnalysis;

namespace Langt;

public struct MessageSeverity
{
    public MessageSeverityType SeverityType {get; init;}
    [MemberNotNullWhen(true, nameof(IsDebug))] public string? Flag {get; init;}

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is not null && obj is MessageSeverity m 
        && m.SeverityType == this.SeverityType
        && m.Flag == this.Flag;

    public override int GetHashCode()
        => HashCode.Combine(SeverityType, Flag);

    public static bool operator==(MessageSeverity a, MessageSeverity b)
        => a.Equals(b);
    public static bool operator!=(MessageSeverity a, MessageSeverity b)
        => !(a==b);

    private MessageSeverity(MessageSeverityType severityType, string? flag)
    {
        SeverityType = severityType;
        Flag = flag;
    }

    public static MessageSeverity Log => new(MessageSeverityType.Log, null);
    public static MessageSeverity Note => new(MessageSeverityType.Note, null);
    public static MessageSeverity Warning => new(MessageSeverityType.Warning, null);
    public static MessageSeverity Error => new(MessageSeverityType.Error, null);
    public static MessageSeverity Fatal => new(MessageSeverityType.Fatal, null);

    public static MessageSeverity Debug(string flag) => new(MessageSeverityType.Debug, flag);

    public bool IsLog => SeverityType is MessageSeverityType.Log;
    public bool IsNote => SeverityType is MessageSeverityType.Note;
    public bool IsWarning => SeverityType is MessageSeverityType.Warning;
    public bool IsError => SeverityType is MessageSeverityType.Error;
    public bool IsFatal => SeverityType is MessageSeverityType.Fatal;
    public bool IsDebug => SeverityType is MessageSeverityType.Debug;

    public bool ShouldDisplay(IReadOnlySet<string> debugFlags)
        => !IsDebug || debugFlags.Contains(Flag!);
}