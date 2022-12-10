extern sqrt(x: real32) : real32

extern printf(txt: ptr ...) : none
extern scanf(text: ptr ...): int32

let is_prime(x: int32) : bool
{
    let upper: real32 = sqrt(x)
    let i: int32 = 2

    while i < upper
    {
        i = i + 1

        if x % i == 0 
        {
            return false
        }
    }

    return true
}   

let main() : none
{
    let val : int32 = 0

    printf("Enter a number:\n\r")
    scanf("%i", ptrto val)

    if is_prime(val)
    {
        printf("%i is prime", val)
    }
    else
    {
        printf("%i is not prime", val)
    }
}