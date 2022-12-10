extern printf(text ptr ...) none
extern scanf(text ptr ...) int32

let println(text ptr) none
{
    printf(text)
    printf("\n\r")
}

let println_i32(x int32): none
{
    printf("%i", x)
    printf("\n\r")
}

let main(): none
{
    let x: int32 = 0
    let y: int64 = x

    while x < 100
    {
        x = x + 1

        let fizz: bool = x % 3 == 0
        let buzz: bool = x % 5 == 0

        if fizz and buzz
        {
            println("fizzbuzz")
        }
        else if fizz
        {
            println("fizz")
        }
        else if buzz
        {
            println("buzz")
        }
        else
        {
            println_i32(x)
        }
    }
}