namespace sys

# Represents a string of characters
struct string
{
    length int64,
    chars *int8
}

# Constructs a string from a string literal
let make_string(chars *int8) string => string {internal.strlen(chars), chars}

# Prints a string to the console
let print(a string) none
{
    internal.printf(a.chars)
}