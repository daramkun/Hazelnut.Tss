using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Hazelnut.Tss;

public enum ColorKind : byte
{
    Black,
    Red,
    Green,
    Yellow,
    Blue,
    Magenta,
    Cyan,
    White,
    
    HighIntensityBlack,
    HighIntensityRed,
    HighIntensityGreen,
    HighIntensityYellow,
    HighIntensityBlue,
    HighIntensityMagenta,
    HighIntensityCyan,
    HighIntensityWhite,
    
    Black000,
    Stratos,
    NavyBlue,
    DukeBlue,
    MediumBlue,
    Blue00F,
    VeryDarkLimeGreen,
    DarkSlateGray,
    SeaBlue,
    Endeavour,
    TrueBlue,
    BrandeisBlue,
    Ao,
    DeepSea,
    Teal,
    DeepCerulean,
    StrongBlue,
    Azure,
    DarkLimeGreen,
    Jade,
    DarkCyan,
    TiffanyBlue,
    Cerulean,
    DeepSkyBlue,
    StrongLimeGreen,
    Malachite,
    CaribbeanGreen,
    StrongCyan,
    DarkTurquoise,
    VividSkyBlue,
    ElectricGreen,
    SpringGreen,
    GuppieGreen,
    MediumSpringGreen,
    SeaGreen,
    Aqua,
    VeryDarkRed,
    VeryDarkMagenta,
    MetallicViolet,
    DarkViolet,
    ElectricViolet,
    ElectricIndigo,
    BronzeYellow,
    Scorpion,
    Comet,
    DarkModerateBlue,
    SlateBlue,
    CornflowerBlue,
    Avocado,
    GladeGreen,
    SteelTeal,
    SteelBlue,
    ModerateBlue,
    LightBlue,
    DarkGreen,
    ForestGreen,
    SilverTree,
    DarkModerateCyan,
    AquaPearl,
    BlueJeans,
    StrongGreen,
    ModerateLimeGreen,
    CaribbeanGreenPearl,
    Downy,
    ModerateCyan,
    MayaBlue,
    BrightGreen,
    LightLimeGreen,
    VeryLightMalachiteGreen,
    MediumAquamarine,
    DarkAquamarine,
    Aquamarine,
    DeepRed,
    FrenchPlum,
    MardiGras,
    Violet,
    StrongViolet,
    PureViolet,
    Brown,
    DeepTaupe,
    ChineseViolet,
    DarkModerateViolet,
    MediumPurple,
    Blueberry,
    Olive,
    ClayCreek,
    TaupeGray,
    CoolGrey,
    ChetwodeBlue,
    VioletsAreBlue,
    AppleGreen,
    Asparagus,
    DarkSeaGreen,
    PewterBlue,
    LightCobaltBlue,
    Malibu,
    Pistachio,
    Mantis,
    PastelGreen,
    PearlAqua,
    MiddleBlueGreen,
    PaleCyan,
    Chartreuse,
    LightGreen,
    VeryLightLimeGreen,
    MintGreen,
    LimeGreen,
    ElectricBlue,
    BrightRed,
    DarkPink,
    DarkMagenta,
    HeliotropeMagenta,
    VividMulberry,
    ElectricPurple,
    DarkOrangeBrownTone,
    DarkModerateRed,
    DarkModeratePink,
    DarkModerateMagenta,
    RichLilac,
    LavenderIndigo,
    DarkGoldenrod,
    DarkModerateOrange,
    DarkGrayishRed,
    Bouquet,
    Lavender,
    BrightLavender,
    LightGold,
    DarkModerateYellow,
    DarkGrayishYellow,
    SilverFoil,
    GrayishBlue,
    MaximumBluePurple,
    VividLimeGreen,
    ModerateGreen,
    YellowGreen,
    GrayishLimeGreen,
    GrayishCyan,
    PaleBlue,
    Lime,
    GreenYellow,
    VeryLightGreen,
    PaleLimeGreen,
    AeroBlue,
    Celeste,
    StrongRed,
    RoyalRed,
    StrongPink,
    HollywoodCerise,
    DeepMagenta,
    Phlox,
    StrongOrange,
    IndianRed,
    Cranberry,
    ModeratePink,
    ModerateMagenta,
    LightMagenta,
    HarvestGold,
    Copperfield,
    NewYorkPink,
    MiddlePurple,
    LightOrchid,
    VeryLightViolet,
    Goldenrod,
    ModerateOrange,
    Tan,
    GrayishRed,
    PinkLavender,
    PaleViolet,
    Corn,
    ModerateYellow,
    MediumSpringBud,
    GrayishYellow,
    LightSilver,
    PaleLavender,
    PureYellow,
    Canary,
    Mindaro,
    PaleGreen,
    Beige,
    LightCyan,
    RedF00,
    VividRaspberry,
    BrightPink,
    PurePink,
    PureMagenta,
    Fuchsia,
    BlazeOrange,
    PastelRed,
    Strawberry,
    HotPink,
    LightDeepPink,
    PinkFlamingo,
    DarkOrange,
    Salmon,
    Tulip,
    PinkSalmon,
    VeryLightPink,
    BlushPink,
    Orange,
    LightOrange,
    VeryLightOrange,
    Sundown,
    PalePink,
    PaleMagenta,
    Gold,
    Dandelion,
    Khaki,
    PaleOrange,
    Cosmos,
    VeryPaleMagenta,
    YellowFf0,
    LightYellow,
    VeryLightYellow,
    PaleYellow,
    Cream,
    LightWhite,
    
    VampireBlack,
    CodGray,
    EerieBlack,
    RaisinBlack,
    DarkCharcoal,
    MineShaft,
    OuterSpace,
    DarkLiver,
    DavysGrey,
    GraniteGray,
    DimGray,
    SonicSilver,
    Gray,
    PhilippineGray,
    DustyGray,
    SpanishGray,
    DarkGray,
    SilverChalice,
    Silver,
    SilverSand,
    LightGray,
    Gainsboro,
    Platinum,
    VeryLightGray,
}

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
[DebuggerDisplay("Red = {Red}, Green = {Green}, Blue = {Blue} ({ToWebColorString()})")]
public readonly struct Color(byte red, byte green, byte blue) : IEquatable<Color>
{
    public readonly byte Red = red, Green = green, Blue = blue;
    
    public Color(byte grayscale) : this(grayscale, grayscale, grayscale) { }

    public string ToWebColorString() => $"#{Red:X2}{Green:X2}{Blue:X2}";
    public string ToHexString() => $"{Red:X2}{Green:X2}{Blue:X2}";
    public override string ToString() => $"{{Red: {Red:X2}, Green: {Green:X2}, Blue: {Blue}}}";

    public bool Equals(Color other) =>
        Red == other.Red && Green == other.Green && Blue == other.Blue;

    public override bool Equals(object? obj) =>
        obj is Color other && Equals(other);

    public override int GetHashCode() =>
        (Red << 16) | (Green << 8) | Blue;

    public static bool operator ==(Color left, Color right) =>
        left.Equals(right);

    public static bool operator !=(Color left, Color right) =>
        !left.Equals(right);
}

[Serializable]
public unsafe struct ColorTable
{
    private fixed byte _colors[256 * 3];

    public Color this[int index]
    {
        get => Colors[index];
        private set => Colors[index] = value;
    }
    
    public Color this[ColorKind colorKind] => this[(int)colorKind];

    public Span<Color> Colors
    {
        get
        {
            fixed (byte* colors = _colors)
                return MemoryMarshal.Cast<byte, Color>(new Span<byte>(colors, 256 * 3));
        }
    }

    public ColorTable(Span<Color> colors)
    {
        switch (colors.Length)
        {
            case 16:
                colors.CopyTo(Colors[..16]);
                var buffer = Colors[16..];
                foreach (var gc in GenerateOtherColors())
                {
                    buffer[0] = gc;
                    buffer = buffer[1..];
                }
                break;
            
            case 256:
                colors.CopyTo(Colors);
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(colors), "Colors must be 16 or 256 count.");
        }
    }

    public static ColorTable Default => new([
        new Color(0x00, 0x00, 0x00),
        new Color(0x80, 0x00, 0x00),
        new Color(0x00, 0x80, 0x00),
        new Color(0x80, 0x80, 0x00),
        new Color(0x00, 0x00, 0x80),
        new Color(0x80, 0x00, 0x80),
        new Color(0x00, 0x80, 0x80),
        new Color(0xc0, 0xc0, 0xc0),

        new Color(0x80, 0x80, 0x80),
        new Color(0xff, 0x00, 0xff),
        new Color(0x00, 0xff, 0x00),
        new Color(0xff, 0xff, 0x00),
        new Color(0x00, 0x00, 0xff),
        new Color(0xff, 0x00, 0xff),
        new Color(0x00, 0xff, 0xff),
        new Color(0xff, 0xff, 0xff),

        ..

        GenerateOtherColors()
    ]);

    private static IEnumerable<Color> GenerateOtherColors()
    {
        byte[] scale =
        [
            0x00, 0x5f, 0x87, 0xaf, 0xd7, 0xff
        ];
        
        for (var r = 0; r < 6; ++r)
        {
            for (var g = 0; g < 6; ++g)
            {
                for (var b = 0; b < 6; ++b)
                {
                    yield return new Color(scale[r], scale[g], scale[b]);
                }
            }
        }
        
        byte[] scale2 =
        [
            0x08, 0x12, 0x1c, 0x26, 0x30, 0x3a, 0x44, 0x4e, 0x58, 0x62, 0x6c, 0x76,
            0x80, 0x8a, 0x94, 0x9e, 0xa8, 0xb2, 0xbc, 0xc6, 0xd0, 0xda, 0xe4, 0xee,
        ];

        foreach (var gray in scale2)
            yield return new Color(gray);
    }
}