namespace sys::my::code

extern printf(text *u8 ...) none
extern malloc(size usize) ptr
extern free(p ptr) none

#[(
    The entry point of our program.
    Should only be called once.
)]
let main() none
{
    printf("%d", sizeof u64)
}

let test(x Wrapper!<Wrapper!<*u32>>, p Pair!<i32, Wrapper!<u32>>) none 
{}

struct Wrapper!<T>
{
    p T
}

struct Pair!<T, V>
{
    first T,
    second V
}