######################
# EXPERIMENTAL IDEAS #
######################

# block comments
#[
    comment in here
]

# extern body syntax
let printf(text *int8 ...) none extern C
let malloc(size int64) *none extern C
# for simplicity, extern C will be the only valid extern for now

# new function type syntax
let func fn(int32, int32) int32 = op_add

# new expression body syntax
let zero64() int64 => 0

# let everything!
let var = 0
let func() none {}
let struct ztruct {}
let alias aliaz = ztruct
let namespace test {}
let namespace test2

# '::' access for static items
let s sys::string = sys::create_string("Hello")

# unsigned integers
let max uint64 = 10000000000

# hex numbers and digit separators
let hex     = 0xFF
let hexlong = 0xFF_FF_FF

# ascii character literals
let nullchar = '\0'a

# call syntaxes
print("Hello")    # equivalent
"Hello"::print()  # equivalent
"Hello"::print    # equivalent

# demonstration of need for '::'/'.' access distinction:
let struct test {str string}
let str(t test) string => "Goodbye!!"

let t = test{"Hello"}
t.str  # "Hello"
t::str # "Goodbye!!"

# constants
let const PI = 3.141

# non-ascii chararcter support 
# (subject to change with research into string types)
let alias char = int32 #predefined
let nullchar char = '\0'

# private members of namespaces
let namespace sys 
{
    let private malloc(size int64) *none extern
    let private const NULL = 0 as *none
    let private struct ptr_s {value *none}
    let private alias ptr_a = *none
}

sys::print # error! Cannot access private member of namespace 'sys' from outside 'sys'

# enums
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

# doc-comments
#[(
    Documentation
)]
let doc = 0

#[(short documentation)] 
let doc = 0

# generic let struct
let struct[T] array
{
    values *T,
    length int32
}

# generic let func
let func[T]() none {} # cannot be extern

# trait functions
let trait hash(this) int32 #'this' serves as both the parameter and type
let trait print(this) none

#               [param+type]      [param] [type]
let hash for int32(this) int32 => this as this
let hash for int64(this) int32 => this as int32

# multiple function traits
let trait formattable
{
    format(this) string,
    format_basic(this) string
}

# empty traits: error
let trait empty {} # error! Cannot create a trait without any member functions

# generic constraints
let inverted_hash[K hash](item K) int32 => not item::hash

# combining traits / typedefs
let type hashprint = hash & print

# Rust-like enums
let enum OptionalInt32
{
    Some(value int32),
    None
}

let test bool = OptionalInt32::Some(0) is Some(let v)

# generic traits
let trait[T] list
{
    get(this, index int32) T
    set(this, index int32, value T) none

    get_ptr(this) *T
}

let[T] list[T] for array[T]
{
    let get(this, i int32) T         => this[i]
    let set(this, i int32, v T) none => this[i] = v

    let get_ptr(this) *T => this.values
}

let trait[Other, Result] op_add(this, other Other) Result

# properties
let prop sign uint8 for some_struct
{
    # 'this' is in scope, both in type and variable form
    let get . . . 
    let set . . . #'value' is also in scope here, as the operand of '='
}

var::sign = var::some_method::sign

# properties with parameters
let prop[index int32] uint8 for some_struct
{
    # 'this' is in scope, but so is 'index'
    let get . . .
    let set . . .
}

var[0] = var::some_method[0]

# 'some' constraint and type inference
let get_first[L some list](l L) L::Key => l::get(0)

# 'where' constraint
let get_first_hash[L some list where L::Key hash](l L) L::Key => l::get_first::hash
let get_first_hash_print[L some list where L::Key hash where L::Key print](l L) L::Key . . . 