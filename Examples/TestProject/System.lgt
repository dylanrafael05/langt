namespace sys

struct string
{
    length int64,
    chars *int8
}

let make_string(chars *int8) string = string {internal.strlen(chars), chars}
let add(a string, b string) string
{
    let newlen = a.length + b.length
    let newchars = internal.malloc(newlen+1) as *int8

    let i = 0
    while i < newlen
    {
        if i < a.length
        {
            newchars[i] = a.chars[i]
        }
        else
        {
            newchars[i] = b.chars[i - a.length]
        }

        i = i + 1
    }

    newchars[i] = 0

    return string {newlen, newchars}
}

let add(a int32, b int32) int32 = a + b 
# TODO: fix this error!
# there should be no issue with the above "redefinition" because it involves a different signature

let print(a string) none
{
    internal.printf(a.chars)
}