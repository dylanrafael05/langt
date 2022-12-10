extern malloc(size int64) *none

let main() none
{
    let arr = malloc(40) as *int32
    arr[1] = 0
    let x = arr[1]
}