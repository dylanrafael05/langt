# langt

Welcome to the repository of my programming language, langt!

![Icon](LANGT_LOGO.png)
> Logo created using [fontstruct](https://fontstruct.com/)

---
Here is the structure of the top-level of folders, including any notable nested folders

- `Core`: contains all of the central components to the compiler
  - `langt-core` is the project which actually holds all of the compiler infrastructure and functionality. Currently, there is no support for `.exe` or `.dll` output, but internally code is transformed to LLVM ir and as such these output modes will be trivial to implement in the future.
  - `langt-cli` is the project which serves as a command-line interface with the compiler
  - `results` is the project which implements the `Result` and `Result<T>` classes for usage throughout the codebase.
- `Examples`: contains example langt files used for testing the compiler and language.
- `Experimental`: contains currently unfinished projects like an implementation of the LSP and a vscode extension
- `Other`: miscellaneous items

---
The code written in this repository uses the C# bindings for LLVM which can be found [here](https://github.com/dotnet/LLVMSharp).
No parser generators or lexers are used for compilation, but TextMate is used to provide simple lexical highlighting for vscode in `Experimental/langt-vscode`

---
## Usage

To use the langt compiler, you must build the project `langt-cli`:
```bash
cd Core/langt-cli
dotnet build
```

The interface for the compiler currently only supports one command, `run`:
```
langt-cli run <filename or directoryname to run> [options]
```
---
## Future Plans / Roadmap

Langt is currently under development, and many features are on their way (see the `function-pointers` branch for unfinished progress).

Future plans, in approximate order of future completion, are as follows:

---
### Implementation of langt-core
- [ ] Reimplement `langt-core` using a `Result`-like type. The current codebase relies heavily on passing an `err` parameter down the syntax tree in order to control whether or not errors and warnings are produced. This makes writing code that handles errors tedious. In order to mitigate this, all functions which can produce diagnostics like `Error: undefined variable` should be reimplemented using a custom `Result` or `Result<ReturnType>` class which will hold a value (or a non-value, if a function would otherwise return `void`), as well as diagnostic and possible error information to propogate up the syntax tree. *Status: implemented on branch `core-rewrite`*

- [ ] Divide phases of semantic analysis into `ASTNode` passes and `BoundASTNode` passes. For instance, this would entail making preliminary checks via `FunctionDefinition.HandleDefinitions()`, then creating a `BoundFunctionDefinition` while performing type-checking, and finally lowering to LLVM-IR with `BoundFunctionDefinition.LowerSelf()`. *Status: implemented on branch `core-rewrite`, requires bugfixes*

---
### Command Line Features
- [x] Add better support for debugging `langt-cli`. *Status: implemented on branch `core-rewrite` by means of a 'defer' command for the cli*

- [ ] Add support to output files as `.o` files or `.dll` files. *Status: unstarted*

- [ ] Add support for configuration files like C#'s `.csproj`. *Status: unstarted*

- [ ] Add pre-built versions of `langt-cli` to top-level of repository. *Status: unstarted*

---
### Language Features
- [ ] Add support for *block comments*: 
```julia
#[ 
    block comment 
]
```
*Status: unstarted*

- [ ] Add support for attaching comments to `ASTToken`s. *Status: unstarted*

- [ ] Modify `extern` syntax to align with `let` function declarations: 
```julia
let printf(text *int8 ...) none extern C
```
*Status: unstarted*

- [x] Bind operators to 'magic methods' in the root namespace which can be overloaded to create user-specified operators: 
```julia
let op_add(a my_struct, b my_struct) my_struct . . .
```
*Status: implemented on branch `core-rewrite`*

- [ ] Implement function pointer types: 
```julia
let func fn(int32, int32) int32
``` 
Add support for target typing function overloads to function pointers: 
```julia
func = op_add
```
*Status: semi-implemented on branch `core-rewrite`*

- [ ] Modify expression body syntax to align with other modern languages: 
```julia
let zero() int64 => 0
```
*Status: semi-implemented on branch `core-rewrite`*

- [ ] Modify definition syntax to be consistent: 
```julia
let var = 0
let func() none {}
let struct ztruct {}
let alias aliaz = ztruct
let namespace test {}
let namespace test2
```
*Status: unstarted*

> Note: all items beyond this point are unimplemented.

- [ ] Change static access to use `::` like in Rust and C++: 
```julia
let s sys::string = sys::create_string("Hello")
```

- [ ] Implement unsigned integer types `uintN` and their operators / conversions

- [ ] Implement hex literals and digit separators: 
```py
let hex     = 0xFF
let hexlong = 0xFF_FF_FF
```

- [ ] Implement ASCII character literals: 
```julia
let nullchar int8 = '\0'a
```

- [ ] Add support for pseudo-object-oriented call syntaxes (calls which specify the first parameter as an object to access from): 
```julia
print("Hello")    # equivalent
"Hello"::print()  # equivalent
"Hello"::print    # equivalent
# this would work for any definition of print

# NOTE: '::' must be used here to avoid ambiguity
# demonstration of need for '::'/'.' access distinction:
let struct test {str string}
let str(t test) string => "Goodbye!!"

let t = test{"Hello"}
t.str  # "Hello"
t::str # "Goodbye!!"
```

- [ ] Add support for constants in the global and local scope: 
```julia
let const PI = 3.141
```

- [ ] Add support for a `char` alias type wide enough to fit all unicode characters (subject to change with more research into Unicode and string representation): 
```rust
let alias char = int32 #predefined
let const nullchar char = '\0'
```

- [ ] Add support for private members of namespaces: 
```julia
let namespace sys 
{
    let private malloc(size int64) *none extern C
    let private const NULL = 0 as *none
    let private struct ptr_s {value *none}
    let private alias ptr_a = *none
}

let n = sys::malloc(0) # error! Cannot access private member 'malloc' of namespace 'sys' from outside 'sys'
```

- [ ] Add support for `sizeof[x]`:
```julia
let const char_size = sizeof[char]
```

- [ ] Add support for simple `enum` types: 
```rust
let enum Day 
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}
```

- [ ] Add support for documentation comments: 
```julia
#[(
    Documentation
)]
let doc = 0

#[(short documentation)] 
let doc = 0
```

- [ ] Add support for generics: 
```julia
# generic let struct
let struct[T] array
{
    values *T,
    length int32
}

# generic let func
let func[T]() none {} # cannot be extern
```

- [ ] Add support for `traits`, which require certain function overloads to exist and have specified return types. 
```julia
# trait functions
let trait hash(this) int32 #'this' serves as both the parameter and type
let trait print(this) none

# the type of this is 'thistype'
let hash for int32(this) int32 => this as thistype
let hash for int64(this) int32 => this as int32

# thistype can be included in the signature of a trait
let trait copy(this) thistype 

# multiple function traits
let trait formattable
{
    format(this) string,
    format_basic(this) string
}

# empty traits: error
let trait empty {} # error! Cannot create a trait without any member functions
```

- [ ] Add support for generic constraints using traits: 
```julia
let inverted_hash[K hash](item K) int32 => not item::hash
```

> Note: there are more ideas for later features which can be found in `core-rewrite/Examples/Playground/roadmap.lgt`. These features are not worth mentioning here because of how far into the future they are.

---
### General

- [ ] Create a wiki which provides basic information about the language and its usage. *Status: started*

- [ ] Fix bugs. *Status: started on branch `rewrite-core`, but unfinished*

- [ ] Improve error messages. *Status: unstarted*

- [ ] Consider adding garbage collection or rust-style ownership. *Status: unstarted*
