using System.Runtime.InteropServices;

namespace Hazelnut.Tss;

public enum FontKind
{
    Primary,
    
    Alternative1,
    Alternative2,
    Alternative3,
    Alternative4,
    Alternative5,
    Alternative6,
    Alternative7,
    Alternative8,
    Alternative9,
}

[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct FontTable
{
    private static readonly string DefaultFont = string.Empty;
    
    public static FontTable Default { get; } = new();
    
    private string Primary = DefaultFont;
    private string Alternative1 = DefaultFont;
    private string Alternative2 = DefaultFont;
    private string Alternative3 = DefaultFont;
    private string Alternative4 = DefaultFont;
    private string Alternative5 = DefaultFont;
    private string Alternative6 = DefaultFont;
    private string Alternative7 = DefaultFont;
    private string Alternative8 = DefaultFont;
    private string Alternative9 = DefaultFont;

    public string this[FontKind kind]
    {
        get
        {
            return kind switch
            {
                FontKind.Primary => Primary,
                FontKind.Alternative1 => Alternative1,
                FontKind.Alternative2 => Alternative2,
                FontKind.Alternative3 => Alternative3,
                FontKind.Alternative4 => Alternative4,
                FontKind.Alternative5 => Alternative5,
                FontKind.Alternative6 => Alternative6,
                FontKind.Alternative7 => Alternative7,
                FontKind.Alternative8 => Alternative8,
                FontKind.Alternative9 => Alternative9,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };
        }
    }

    public string this[int index] => this[(FontKind)index];

    public FontTable(Span<string> fonts)
    {
        if (fonts.Length > 0) Primary = fonts[0];
        if (fonts.Length > 1) Alternative1 = fonts[1];
        if (fonts.Length > 2) Alternative2 = fonts[2];
        if (fonts.Length > 3) Alternative3 = fonts[3];
        if (fonts.Length > 4) Alternative4 = fonts[4];
        if (fonts.Length > 5) Alternative5 = fonts[5];
        if (fonts.Length > 6) Alternative6 = fonts[6];
        if (fonts.Length > 7) Alternative7 = fonts[7];
        if (fonts.Length > 8) Alternative8 = fonts[8];
        if (fonts.Length > 9) Alternative9 = fonts[9];
    }
}