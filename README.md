# langt

Welcome to the repository of my programming language, langt!

I am relatively new to effective git-hub usage, and as such there will be many idiosyncracies and poor practices found inside this repository.

---
Here is the structure of the top-level of folders, including any notable nested folders

- `Core`: contains all of the central components to the compiler
  - `langt-core` is the project which actually holds all of the compiler infrastructure and functionality. Currently, there is no support for `.exe` or `.dll` output, put internally code is transformed to LLVM ir and as such these output modes will be trivial to implement in the future.
  - `langt-cli` is the project which serves as a command-line interface with the compiler
- `Examples`: contains example langt files used for testing the compiler and language alike.
- `Expiremental`: contains currently unfinished projects like a LSP and vscode extension
- `Other`: holds all other ramblings 

---
The code written here is based on the C# bindings for LLVM which can be found [here](https://github.com/dotnet/LLVMSharp)