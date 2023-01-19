namespace sys

extern printf(text *int8 ...) none

let maybe_int() int8 | int64 => 0 as int64

#[(
    Doc Comment
)]
let main() none
{
    let x = "Hello World"

    printf(x)
    sys::printf("hello")
}