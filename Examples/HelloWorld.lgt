extern printf(text *int8 ...) none

struct string 
{
    chars *int8,
    len int64
}

let make_str(chars *int8) string -> string {chars, }

let main() none 
{
    printf("%d", eval(two))
}