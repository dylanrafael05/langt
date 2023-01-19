namespace sys

extern printf(text *int8 ...) none

#[(
    Doc Comment
)]
let main() none
{
    let x = "Hello World"

    printf(x)
    sys::printf("hello")
}