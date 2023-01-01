using sys

let op_add(a string, b string) string
{
    let newlen = a.length + b.length
    let newchars = internal.malloc(newlen+1) as *int8

    let a = 0

    let i = 0
    while i < newlen
    {
        # test comment
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

    let x = 0
}