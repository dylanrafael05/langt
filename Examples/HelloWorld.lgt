extern printf(text *u8 ...) none

let main() none
{
    let x = str {0}
    printf("%d", x.o + 9)
}

#[
    This is a documentation comment
]
struct str 
{
    x i64
}