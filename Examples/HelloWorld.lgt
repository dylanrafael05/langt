# Print to the console
extern printf(text *int8 ...) none

# Our main function
let main() none
{
    # Variable comment
    let x = "I'm ALIIIIIIIVE!!!"

    let test = 0
    let a = 0
    test = test + a

    printf(x)
}

#[(
    My struct comment
    is the very best struct comment
)]
struct test 
{
    x int32
}

let make_test() test => test {0}