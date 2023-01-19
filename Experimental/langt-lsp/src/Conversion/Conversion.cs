namespace Langt.LSP;

public static class Conversion
{
    public static VSDiagnostic ToVS(this LangtDiagnostic d) 
        => new() 
        {
            Message = d.Message, //+ ":@" + d.Range.RawRepresentation,
            Range = d.Range.ToVS(),
            Source = "langt-vscode",
            Severity = d.Severity.SeverityType.ToVS()
        };

    public static VSDiagnosticSeverity ToVS(this LangtDiagnosticSeverity d) 
        => d switch 
        {
            LangtDiagnosticSeverity.Error   => VSDiagnosticSeverity.Error,
            LangtDiagnosticSeverity.Warning => VSDiagnosticSeverity.Warning,
            LangtDiagnosticSeverity.Note    => VSDiagnosticSeverity.Hint,
            _                               => VSDiagnosticSeverity.Information
        };

    public static VSRange ToVS(this LangtRange r) 
        => new(r.Start.ToVS(), r.End.ToVS());
    public static VSPosition ToVS(this LangtPosition p)
        => new(p.Line-1, p.Column);

    public static LangtRawPosition ToLangt(this VSPosition p) 
        => new(p.Line+1, p.Character);
}