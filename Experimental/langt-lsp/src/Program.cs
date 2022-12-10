namespace Langt.LSP;

#pragma warning disable VSTHRD002

public class Program
{
    private static void Main()
    {
        MainAsync().Wait();
    }

    private static async Task MainAsync()
    {
        var server = await LangtLanguageServer.BuildServerAsync();
        await server.WaitForExit.ConfigureAwait(false);
    }
}

