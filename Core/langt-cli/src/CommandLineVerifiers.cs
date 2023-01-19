using System.CommandLine;

namespace Langt.CLI;

public static class CommandLineVerifiers
{
    public static Argument<string> ValidFilePathOrDirectory(this Argument<string> a, params string[] acceptedFileExtensions)
    {
        a.AddValidator(r => 
        {
            var filename = r.GetValueOrDefault<string>();

            if(!File.Exists(filename))
            {
                if(!Directory.Exists(filename))
                {
                    r.ErrorMessage = "Could not find " + filename + "" + Environment.NewLine + "Please enter a valid filename";
                }
            }
            else if(acceptedFileExtensions.Length == 0) return;
            else if(!acceptedFileExtensions.Contains(Path.GetExtension(filename)))
            {
                r.ErrorMessage = "Input file must have an extension of " + Readable.CommaListOr(acceptedFileExtensions);
            }
        });

        return a;
    }
}