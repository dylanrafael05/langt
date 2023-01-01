namespace Langt.LSP;

public class Program
{
    private static async Task Main()
    {
        var server = await LangtLanguageServer.BuildServerAsync();
        await server.WaitForExit;
    }
}

