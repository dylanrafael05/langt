let main() none 
{
    let text = sys.add(sys.make_string("Hello "), sys.make_string("World!"))
    sys.print(text)

    let x = sys.add(0, 0)
    sys.internal.scanf("%d", &x)
    sys.internal.printf("%d", x)
}