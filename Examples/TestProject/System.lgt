namespace sys

struct string
{
    length int64,
    chars *int8
}

let make_string(chars *int8) string -> string {internal.strlen(chars), chars}

let print(a string) none
{
    internal.printf(a.chars)
}