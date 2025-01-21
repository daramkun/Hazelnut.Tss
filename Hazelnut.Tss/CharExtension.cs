namespace Hazelnut.Tss;

internal static class CharExtension
{
#if NETSTANDARD2_1
    public static bool IsAsciiLetter(this char ch) => ch is >= 'a' and <= 'z' or >= 'A' and <= 'Z';
    public static bool IsAsciiDigit(this char ch) => ch is >= '0' and <= '9';
#else
    public static bool IsAsciiLetter(this char ch) => char.IsAsciiLetter(ch);
    public static bool IsAsciiDigit(this char ch) => char.IsAsciiDigit(ch);
#endif
}