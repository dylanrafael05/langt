extern printf(text *u8 ...) none
extern malloc(size usize) ptr
extern free(p ptr) none

let main() none
{
    printf("%d", sizeof u64)
}

let k(a u8, b u8) u8 => a