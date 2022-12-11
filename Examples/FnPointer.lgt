extern printf(text *int8 ...) none

let apply(a int32, b int32, fn *(int32, int32) int32) int32
    = fn(a, b)

let add(a int32, b int32) int32 = a + b
let sub(a int32, b int32) int32 = a - b

let main() none 
{
    let x 
    printf("1 + 1 = %d\n\r", apply(1, 1, add))
    printf("1 - 1 = %d\n\r", apply(1, 1, sub))
}