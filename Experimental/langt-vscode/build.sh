# Initialize
printf $"Beginning build . . . \n"

# Compile typescript
printf $"Compiling typescript . . . \n"
tsc -p ./

# Compile langt-lsp
printf $"Building lsp implementation . . . \n\n"
cd ../langt-lsp
dotnet build

# Return to cwd
printf $"\nDone!\n"
cd ../langt-vscode