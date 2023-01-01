using MediatR;
using OmniSharp.Extensions.JsonRpc;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;
using OmniSharp.Extensions.LanguageServer.Server;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Serilog;

namespace Langt.LSP;

public class LangtLanguageServer
{
    public static async Task<LanguageServer> BuildServerAsync() 
    {
        var server = await LanguageServer.From(
            server => server
                .WithInput(Console.OpenStandardInput())
                .WithOutput(Console.OpenStandardOutput())
                .WithLoggerFactory(new LoggerFactory())
                .AddDefaultLoggingProvider()
                .WithServices(Services)
                .AddHandler<TextDocumentHandler>()
        );

        return server;
    }

    public static void Services(IServiceCollection s) 
    {
        s.AddSingleton(new ProjectManager());
    }

    public const string LanguageID = "langt";
    
    public static DocumentSelector DocumentSelector => DocumentSelector.ForPattern("**/*.lgt");
    public static TextDocumentSyncKind SyncKind => TextDocumentSyncKind.Full;
}

