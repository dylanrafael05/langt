using System.Collections;

namespace Langt;

public class DiagnosticCollection : ICollection<Diagnostic>
{
    public void AddAll(DiagnosticCollection other) 
    {
        foreach(var db in other)
        {
            Add(db);
        }
    }

    public bool AnyErrors => ErrorCount > 0;
    public int ErrorCount {get; private set;}

    public DiagnosticCollection() {}

    private List<Diagnostic> items = new();

    public int Count => items.Count;
    public bool IsReadOnly => false;

    public void Note(string message, SourceRange range)
        => Add(new(DiagnosticSeverity.Note, message, range));
    public void Warning(string message, SourceRange range)
        => Add(new(DiagnosticSeverity.Warning, message, range));
    public void Error(string message, SourceRange range)
        => Add(new(DiagnosticSeverity.Error, message, range));
    public void Fatal(string message, SourceRange range)
        => Add(new(DiagnosticSeverity.Fatal, message, range));

    public void Add(Diagnostic item)
    {
        items.Add(item);
        items = items.OrderBy(i => i.Range.Source.Name).ThenBy(i => i.Range.CharStart).ToList();

        if(item.Severity >= DiagnosticSeverity.Error) ErrorCount++;
    }

    public void Clear()
    {
        items.Clear();
    }

    public bool Contains(Diagnostic item)
    {
        return items.Contains(item);
    }

    public void CopyTo(Diagnostic[] array, int arrayIndex)
    {
        items.CopyTo(array, arrayIndex);
    }

    public bool Remove(Diagnostic item)
    {
        if(items.Remove(item))
        {
            if(item.Severity >= DiagnosticSeverity.Error) ErrorCount--;
            return true;
        }

        return false;
    }

    public IEnumerator<Diagnostic> GetEnumerator()
        => items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
