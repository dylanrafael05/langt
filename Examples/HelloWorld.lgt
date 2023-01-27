extern printf(text *u8 ...) none


#[(
    You suck
)]
let main() none
{
    printf("HALLO WARLD")
    let x = identity(69)

    let k = &x
    x = *k
}

let identity(x i8) u8 => x as u8