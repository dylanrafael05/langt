extern printf(text *u8 ...) none

let main() none
{
    printf("%d", sizeof u64)
}

struct Test!<T>
{
    a T
}