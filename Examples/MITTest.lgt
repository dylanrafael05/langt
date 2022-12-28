extern printf(text *int8 ...) none
extern malloc(sz int64) *none
extern free(p *none, sz int64) none

let init_primes(n int64, p *bool) none
{
    let i = 0
    while i < n
    {
        p[i] = true
        i = i + 1
    }
}

let free_primes(n int64, p *bool) none
{
    free(p as *none, n)
}

let nullify_prime_mults(n int64, i int32, p *bool) none
{
    if p[i-1] 
    {
        let j = 2
        while j * (i-1) < n
        {
            p[j * (i-1)] = false
            j = j + 1
        }
    }
}

let primes(n int64) *bool
{
    let p = malloc(n) as *bool
    init_primes(n, p)

    let i = 2

    # Loop through all numbers between '2' and 'n'
    while i < n 
    {
        # If the current value is a prime, remove all multiples greater than one
        nullify_prime_mults(n, i, p)
        i = i + 1
    }

    # Return the primes
    return p
}

let print_primes(n int64, p *bool) none
{
    let i = 0
    while i < n
    {
        i = i + 1
        if p[i-1]
        {
            printf("%d is prime!\n\r", i+1)
        }
    }
}

let main() none
{
    let count = 1000

    let p = primes(count)

    print_primes(count, p)

    free_primes(count, p)
}