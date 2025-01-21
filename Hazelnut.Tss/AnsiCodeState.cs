namespace Hazelnut.Tss;

public enum BlinkKind
{
    None,
    Slow,
    Fast,
}

public enum SuperOrSubscript
{
    Default,
    Superscript,
    Subscript,
}

[Serializable]
public struct AnsiCodeState : IEquatable<AnsiCodeState>
{
    public bool IsBold;
    public bool IsFaint;
    public bool IsItalic;
    public bool IsUnderline;
    public bool IsStrikeThrough;
    public bool IsOverline;

    public BlinkKind Blink;
    public SuperOrSubscript SuperOrSubscript;

    public bool DefaultForeground = true;
    public bool DefaultBackground = true;
    
    public Color Foreground;
    public Color Background;

    public string? FontFamily;
    public string? HyperlinkUrl;

    public AnsiCodeState() { }

    public bool IsDefault => Equals(new AnsiCodeState());

    public void ResetGraphic()
    {
        IsBold = IsFaint = IsItalic = IsUnderline = IsStrikeThrough = IsOverline = false;
        Blink = BlinkKind.None;
        SuperOrSubscript = SuperOrSubscript.Default;
        DefaultForeground = DefaultBackground = true;
        FontFamily = null;
    }

    public bool Equals(AnsiCodeState other)
    {
        if (IsBold != other.IsBold ||
            IsFaint != other.IsFaint ||
            IsItalic != other.IsItalic ||
            IsUnderline != other.IsUnderline ||
            IsStrikeThrough != other.IsStrikeThrough ||
            IsOverline != other.IsOverline ||
            Blink != other.Blink ||
            SuperOrSubscript != other.SuperOrSubscript ||
            DefaultForeground != other.DefaultForeground ||
            DefaultBackground != other.DefaultBackground ||
            !StringEquals(FontFamily, other.FontFamily) ||
            !StringEquals(HyperlinkUrl, other.HyperlinkUrl))
            return false;

        if (!DefaultForeground)
        {
            if (!Foreground.Equals(other.Foreground))
                return false;
        }

        if (!DefaultBackground)
        {
            if (!Background.Equals(other.Background))
                return false;
        }

        return true;
    }

    public override bool Equals(object? obj) =>
        obj is AnsiCodeState other && Equals(other);

    private static bool StringEquals(string? a, string? b)
    {
        if (a is null or "" && b is null or "")
            return true;
        return a?.Equals(b) == true;
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(IsBold);
        hashCode.Add(IsFaint);
        hashCode.Add(IsItalic);
        hashCode.Add(IsUnderline);
        hashCode.Add(IsStrikeThrough);
        hashCode.Add(IsOverline);
        hashCode.Add((byte)Blink);
        hashCode.Add((byte)SuperOrSubscript);
        hashCode.Add(DefaultForeground);
        hashCode.Add(DefaultBackground);
        hashCode.Add(DefaultForeground ? new() : Foreground);
        hashCode.Add(DefaultBackground ? new() : Background);
        hashCode.Add(!string.IsNullOrEmpty(FontFamily) ? FontFamily : null);
        hashCode.Add(!string.IsNullOrEmpty(HyperlinkUrl) ? HyperlinkUrl : null);
        return hashCode.ToHashCode();
    }

    public static bool operator ==(AnsiCodeState left, AnsiCodeState right) =>
        left.Equals(right);

    public static bool operator !=(AnsiCodeState left, AnsiCodeState right) =>
        !left.Equals(right);
}