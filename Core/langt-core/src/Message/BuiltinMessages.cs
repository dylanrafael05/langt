namespace Langt.Message;

public static class BuiltinMessages
{
//TODO: move this to a referenced file rather than a C# source file
//add syntax highlighting and more features, like '*' after types to allow for automatic calls to .Stringify()
//i.e. `many-problems, probs:str* = There are many problems: $probs`
public const string Source =
"""
int-range, val:str, type:qname = Integer $value out of range for type $type

unknown-char, ch:str = Unknown character '$ch'
escape-char, ch:str = Unrecognized string escape sequence '\$ch'

direct-REMOVE-ME, txt:str = $txt

expected, ty:str = Expected to find a $ty but did not
block-comment = Unterminated block comment

fn-overload-redefine, fn:qname, sig:str = Cannot redefine overload of function $fn with signature $sig
fn-no-matching-overload, fn:qname, types:str = Could not resolve any matching overloads for call to $fn with parameter $types
fn-multiple-matching-overload, fn:qname, types:str = Could not resolve any one matching overload for call to $fn with parameter $types, multiple overloads are valid
fn-ref-impossible = Cannot target type a function reference to a non-functional type
fn-bad-arg-count, ty:qname = Incorrect number of arguments to call to function of type $ty
fn-not-extern-vararg = Cannot define a function as variable arguments if it is not extern
fn-bad-ptr-call, ty:qname, args:str = Cannot call a function pointer of type $ty with arguments of type $args

call-non-fn = Cannot call a non-functional expression

no-found, name:str = No item named $name found in scope
redefine, name:str = Cannot redefine name $name

field-redefine, ty:qname, name:str = Cannot redefine field $ty.$name
field-cyclic, ty:qname, name:str = Field $ty.$name causes a cyclic struct layout

alias-recursion, ty:qname = Alias type $ty cannot be recursive

option-dup, ty:qname, opt:qname = Duplicate type $ty in option type $opt

ptr-to-ref = Cannot create a pointer to a reference type
ref-ro-ref = Cannot create a reference to a reference type

struct-init-not-struct, ty:qname = Cannot initialize the non structure type $ty with a struct initializer
struct-init-bad-arg-count, ty:qname, ec:str, ac:str = Incorrect number of fields for structure initializer of type $ty; expcted $ec but got $ac
struct-init-bad-arg-ty, ty:qname, f:str, et:qname = Incorrect type for field $ty.$f, expected $et

dot-not-struct, ty:qname = Cannot use a '.' expression on non-struct type $ty
dot-bad-field, ty:qname, f:str = Unknown field $ty.$f

cc-bad-lhs = Static access '::' requires a scope as its left-hand operand

assign-bad-lhs = Can only assign to assignable values
assign-bad-index-lhs = Can only index-assign to assignable values

ptr-bad-deref = Cannot dereference a non-pointer

ty-not-constructed, ty:qname = Cannot use unconstructed type $ty in this context

conversion-no-found, ipt:qname, opt:qname = Could not find a conversion from $ipt to $opt
conversion-no-found-explicit, ipt:qname, opt:qname = Could not find conversion from $ipt to $opt (an explicit conversion exists)

expected-generic, ty:qname = Expected a generic type, got $ty
generic-bad-arg-count, ty:qname, args:str, ec:str, ac: str = Cannot construct generic type from $ty with arguments $args; expected $ec arguments, not $ac
generic-ref-arg = Cannot supply a reference type to a generic type

unreachable = Code here is unreachable
""";
}