extern printf(text *int8 ...) none
extern malloc(size int64) *none

struct int32Array 
{
    elements *int32, 
    size int32
}

let makei32a(size int32) int32Array = int32Array {malloc(size*4) as *int32, size}

let foreach(arr int32Array, fn *(int32) int32) none
{
    let x = 0
    while x < arr.size 
    {
        arr.elements[x*4] = fn(arr.elements[x*4])
        x = x + 1
    }
}
let foreach(arr int32Array, fn *(int32, int32) int32) none
{
    let x = 0
    while x < arr.size 
    {
        arr.elements[x*4] = fn(arr.elements[x*4], x)
        x = x + 1
    }
}

let ten(n int32, i int32) int32 = i
let print_item(n int32) int32 
{
    printf("%d\n\r", n)
    return n
}

let main() none
{
    printf("Begin! \n\r")

    let arr = makei32a(10)

    foreach(arr, ten)
    foreach(arr, print_item)
}