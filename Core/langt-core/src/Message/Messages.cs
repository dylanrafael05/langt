using System.Reflection;
using System.Text.RegularExpressions;
using Langt.Structure;

namespace Langt.Message;

public enum MessageFieldType
{
    String,
    Name,
    QualName
}

public static class MessageFieldTypeExtensions
{
    public static string Handle(this MessageFieldType type, object value) => type switch
    {
        MessageFieldType.Name     => Expect.Is<IFullNamed>(value).DisplayName,
        MessageFieldType.String   => value.ToString()!,
        MessageFieldType.QualName => Expect.Is<IFullNamed>(value).FullName,

        _ => throw new UnreachableException()
    };
}

public record struct MessageFieldSpec(string Name, MessageFieldType Type);
public record struct MessageField(string Name, MessageFieldType Type, int Index);

public interface IMessageSegment 
{
    string Handle(MessageBuilder builder, object[] values);
}

public record struct TextSegment(string Text) : IMessageSegment
{
    public string Handle(MessageBuilder builder, object[] values)
        => Text;
}
public record struct FieldSegment(string FieldName) : IMessageSegment
{
    public string Handle(MessageBuilder builder, object[] values)
    {
        var cfield = builder.Fields[FieldName];

        return cfield.Type.Handle(values[cfield.Index]);
    }
}

public class MessageBuilder
{
    public string Name {get;}
    public int ID {get;}

    public IReadOnlyDictionary<string, MessageField> Fields {get;}
    public IReadOnlyList<IMessageSegment> Segments {get;}
    
    public MessageBuilder(string name, int id, MessageFieldSpec[] fields, IMessageSegment[] segments) 
    {   
        Name = name;
        ID   = id;

        Fields   = fields.Indexed().Select(p => new MessageField(p.Value.Name, p.Value.Type, p.Index)).ToDictionary(k => k.Name);
        Segments = segments.ToArray();
    }

    public string Handle(params object[] values)
    {
        Expect.That(values.Length == Fields.Count, "Message builders must receive exactly their number of fields as arguments to .Handle()");

        return string.Concat(Segments.Select(s => s.Handle(this, values)));
    }
}

public static partial class MessageParser
{
    public static int MaxID {get; private set;} = 0;

    public const char FieldChar = '$';
    public const char Declarator = '=';
    public const char Comma = ',';
    public const char Type = ':';

    public static IEnumerable<IMessageSegment> ParseSegments(string text) 
    {
        var cur = 0;
        var last = 0;

        var anyCSeen = false;
        
        char Get() {return text[cur];}
        void Pop() {last = cur;}
        void Move() {cur++;}
        bool AtEnd() {return cur >= text.Length;}

        void ThrowIf(bool b, string msg) {if(b) throw new Exception(msg);}

        IEnumerable<IMessageSegment> CheckText()
        {
            if(anyCSeen)
            {
                yield return new TextSegment(text[last..cur]);

                Pop();
            }
        }

        while(!AtEnd())
        {
            if(Get() is FieldChar)
            {
                foreach(var m in CheckText()) yield return m;
                
                Move();
                ThrowIf(AtEnd(), $"Empty field found in message body {text}");

                if(Get() is FieldChar)
                {
                    yield return new TextSegment("" + FieldChar);

                    Move(); 
                    Pop();
                }
                else 
                {
                    while(!AtEnd() && char.IsLetter(Get())) Move();

                    yield return new FieldSegment(text[(last+1)..cur]);
                    Pop();
                }

                anyCSeen = false;
            }
            else 
            {
                Move();
                anyCSeen = true;
            }
        }
        
        foreach(var m in CheckText()) yield return m;
    }

    public static MessageBuilder ParseMessage(string text) 
    {
        var splitByDec = text.Split(Declarator);
        Expect.That(splitByDec.Length == 2);

        var (def, seg) = (splitByDec[0].Trim(), splitByDec[1].Trim());

        var defSegs = def.Split(Comma);
        Expect.That(defSegs.Length >= 1);

        var name = defSegs[0].Trim();

        var args = new List<MessageFieldSpec>();
        foreach(var rest in defSegs.Skip(1))
        {
            var tySplit = rest.Split(Type);
            Expect.That(tySplit.Length == 2);

            var (argName, argTyStr) = (tySplit[0].Trim(), tySplit[1].Trim());

            Expect.That(argName.All(char.IsLetter));

            var argTy = argTyStr switch 
            {
                "qname" => MessageFieldType.QualName,
                "name"  => MessageFieldType.Name,
                "str"   => MessageFieldType.String,
                _ => throw new Exception($"Unknown message item type {argTyStr}")
            };

            args.Add(new(argName, argTy));
        }

        var segs = ParseSegments(seg).ToArray();

        return new(name, MaxID++, args.ToArray(), segs);
    }

    public static IEnumerable<MessageBuilder> Parse(string text) 
    {
        foreach(var line in Regex.Split(text, @"\r\n|\r|\n")
                           .Select(s => s.Trim())
                           .Where(s => s != "" && !s.StartsWith('#')))
        {
            yield return ParseMessage(line);
        }
    }
}

public static class Messages
{
    private static bool IsInit {get; set;}
    private static void Init()
    {
        IncludeAllFromString(BuiltinMessages.Source);
        IsInit = true;
    }

    private readonly static Dictionary<string, MessageBuilder> messages = new();

    public static void Include(MessageBuilder message) 
        => messages[message.Name] = message;

    public static void IncludeAllFromString(string content) 
    {
        foreach(var msg in MessageParser.Parse(content))
            Include(msg);
    }
    public static void IncludeAllFromFile(string path) 
        => IncludeAllFromString(File.ReadAllText(path));
        
    public static IReadOnlyDictionary<string, MessageBuilder> AllMessages => messages;

    public static MessageBuilder BuilderFor(string name)
    {
        if(!messages.TryGetValue(name, out var mb))
        {
            throw new Exception($"Message named {name} not found!");
        }

        return mb;
    }
    public static int IDFor(string name) 
        => BuilderFor(name).ID;

    public static MsgInfo Get(string name, params object[] formatParams)
    {
        if(!IsInit) Init();
        var builder = BuilderFor(name);
        return new(builder.Handle(formatParams), builder.ID);
    }
}

public record struct MsgInfo(string Message, int ID);