using VSDiagnostic = OmniSharp.Extensions.LanguageServer.Protocol.Models.Diagnostic;
using LangtDiagnostic = Langt.Diagnostic;
using VSDiagnosticSeverity = OmniSharp.Extensions.LanguageServer.Protocol.Models.DiagnosticSeverity;
using LangtDiagnosticSeverity = Langt.MessageSeverityType;
using VSRange = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;
using LangtRange = Langt.SourceRange;
using VSPosition = OmniSharp.Extensions.LanguageServer.Protocol.Models.Position;
using LangtPosition = Langt.SourcePosition;
using LangtRawPosition = Langt.SimplePosition;

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