extern sqrt(x real32) real32

extern printf(txt *int8 ...) none
extern scanf(text *int8 ...) int32

struct PrimeResult
{
    isprime bool,
    factor int32
}

let is_prime(x int32) PrimeResult
{
    let upper = sqrt(x)
    let i = 2

    while i < upper
    {
        i = i + 1

        if x % i == 0 
        {
            return PrimeResult {false, i}
        }
    }

    return PrimeResult {true, 0}
}   

let main() none
{
    let val = 0

    printf("Enter a number:\n\r")
    scanf("%i", &val)

    let r int32 = is_prime(val)

    if not r.isprime
    {
        printf("%i is not prime, and it has a factor of %d", val, r.factor)
    }
    else
    {
        printf("%i is prime", val)
    }
}