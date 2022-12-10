extern printf(text *int8 ...) none

let main() none 
{
    let x int32 = 0
    printf("%i\n\r", x)

    let y real64 = x + 1
    printf("%.1f\n\r", y)

    let z int32 = y as int32 - 100
    printf("%i\n\r", z)
    
    let z1 int64 = y as int32 + 1
    printf("%i\n\r", z1)

    
    let q real64 = 0
    printf("%d\n\r", q)
}