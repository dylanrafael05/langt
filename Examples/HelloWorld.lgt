extern printf(text: ptr ...): none
extern scanf(text: ptr ...): int32

let main() : none 
{
    let grade : real32 = 0

    printf("Enter your grade:\n\r")
    scanf("%f", ptrto grade)

    printf("You got a %f, which is a", grade as real64)

    if grade > 90
    {
        printf("n A")
    }
    else if grade > 80
    {
        printf(" B")
    }
    else if grade > 70
    {
        printf(" C")
    }
    else if grade > 60
    {
        printf(" D")
    }
    else 
    {
        printf("n F")
    }

    printf("!\n\r")
}