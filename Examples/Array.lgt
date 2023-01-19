extern printf(text *int8 ...) none
extern malloc(size int64) *none
extern free(ptr *none) none

struct int32Array 
{
    elements *int32, 
    size int32
}

let makei32a(size int32) int32Array -> int32Array {malloc(size*4) as *int32, size}
let deli32a(arr int32Array) none
{
    free(arr.elements as *none)
}

let foreach(arr int32Array, fn *(int32) int32) none
{
    let x = 0
    while x < arr.size 
    {
        arr.elements[x] = fn(arr.elements[x])
        x = x + 1
    }
}
let foreach(arr int32Array, fn *(int32, int32) int32) none
{
    let x = 0
    while x < arr.size 
    {
        arr.elements[x] = fn(arr.elements[x], x)
        x = x + 1
    }
}

let index(n int32, i int32) int32 -> i
let print_item(n int32) int32 
{
    printf("%d\n\r", n)
    return n
}

let main() none
{
    printf("Begin! \n\r")

    let arr = makei32a(10)

    foreach(arr, index)
    foreach(arr, print_item)

    deli32a(arr)
}