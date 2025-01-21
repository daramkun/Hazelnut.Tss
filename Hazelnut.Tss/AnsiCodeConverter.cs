using System.Diagnostics.CodeAnalysis;
using Hazelnut.Tss.StringBuilders;
using Hazelnut.Tss.Stringifiers;

namespace Hazelnut.Tss;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class AnsiCodeConverter(IStringifier stringifier, in ColorTable colorTable, in FontTable fontTable, 
    IStringBuilderFactory? stringBuilderFactory = null)
{
    private readonly ColorTable _colorTable = colorTable;
    private readonly FontTable _fontTable = fontTable;
    private readonly IStringBuilderFactory _stringBuilderFactory = stringBuilderFactory
                                                                   ?? DefaultStringBuilderFactory.SharedInstance;
    
    private AnsiCodeState _state = new() { FontFamily = fontTable[0] };
    
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public bool MaskPrivacyMessage { get; set; }

    public AnsiCodeConverter()
        : this(HtmlStringifier.SharedInstance, ColorTable.Default, FontTable.Default,
            DefaultStringBuilderFactory.SharedInstance)
    { }

    public AnsiCodeConverter(IStringifier stringifier, IStringBuilderFactory? stringBuilderFactory = null)
        : this(stringifier, ColorTable.Default, FontTable.Default, stringBuilderFactory)
    { }

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public void Convert(IStringBuilder output, string text)
    {
        using var reader = new StringReader(text);
        Convert(output, reader);
    }

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public void Convert(IStringBuilder output, TextReader reader)
    {
        using var buffer = _stringBuilderFactory.Create();
        var parseState = ParseState.Text;

        while (parseState != ParseState.Eof)
        {
            parseState = parseState switch
            {
                ParseState.Text => ParseText(reader, output, buffer),
                ParseState.StartEscape => ParseStartEscape(reader),
                ParseState.SelectGraphicRendition => ParseSelectGraphicRendition(reader, buffer),
                ParseState.OperatingSystemControl => ParseOperatingSystemControl(reader, buffer),
                ParseState.PrivacyMessage => MaskPrivacyMessage
                    ? ParsePrivacyMessage(reader, output, buffer)
                    : ParseState.Text,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (parseState == ParseState.Eof && buffer.Length > 0)
                stringifier.Stringify(output, _state, buffer.AsSpan());
        }
    }

    public string Convert(string text)
    {
        using var output = _stringBuilderFactory.Create();
        Convert(output, text);
        return output.ToString();
    }

    public string Convert(TextReader reader)
    {
        using var output = _stringBuilderFactory.Create();
        Convert(output, reader);
        return output.ToString();
    }

    private ParseState ParseText(TextReader reader, IStringBuilder builder, IStringBuilder buffer)
    {
        if (reader.Peek() == -1)
            return ParseState.Eof;

        var ch = (char)reader.Read();
        if (ch == '\e')
        {
            if (buffer.Length > 0)
                stringifier.Stringify(builder, _state, buffer.AsSpan());
            buffer.Clear();
            return ParseState.StartEscape;
        }

        stringifier.Escape(ch, buffer);
        return ParseState.Text;
    }

    private ParseState ParseStartEscape(TextReader reader)
    {
        switch (reader.Read())
        {
            case 'N': return ParseState.Text; // ParseState.SingleShiftTwo;
            case 'O': return ParseState.Text; // ParseState.SingleShiftThree;
            case 'P': return SkipUntilStringTerminator(reader); // ParseState.DeviceControlString;
            case '[': return ParseState.SelectGraphicRendition;
            case '\\': return ParseState.Text; // ParseState.StringTerminator;
            case ']': return ParseState.OperatingSystemControl;
            case 'X': return ParseState.Text; // ParseState.StartOfString;
            case '^': return ParseState.PrivacyMessage;
            case '_': return SkipUntilStringTerminator(reader); // ParseState.ApplicationProgramCommand;
            case '(':
                _state.FontFamily = _fontTable[FontKind.Primary];
                return SkipOneChar(reader);
            case ')':
                _state.FontFamily = _fontTable[FontKind.Alternative1];
                return SkipOneChar(reader);
            default: return SkipOneChar(reader);
        }
    }

    private static ParseState SkipOneChar(TextReader reader)
    {
        reader.Read();
        return ParseState.Text;
    }

    private static ParseState SkipUntilStringTerminator(TextReader reader)
    {
        var lastCh = (char)reader.Peek();
        int chInt;

        while ((chInt = reader.Read()) != -1)
        {
            var ch = (char)chInt;
            if (lastCh == '\e' && ch == '\x9c')
                return ParseState.Text;

            lastCh = ch;
        }
        
        return ParseState.Eof;
    }

    private ParseState ParseSelectGraphicRendition(TextReader reader, IStringBuilder buffer)
    {
        int read;
        while ((read = reader.Read()) != -1)
        {
            var ch = (char)read;
            buffer.Append(ch);
            if (ch.IsAsciiLetter()) break;
            if (ch.IsAsciiDigit() || ch == ';') continue;

            // Invalid syntax
            buffer.Clear();
            return ParseState.Text;
        }

        if (read == -1)
            return ParseState.Eof;

        // Not support Control Code
        if (buffer[^1] is not 'm')
        {
            buffer.Clear();
            return ParseState.Text;
        }

        Span<int> codes = stackalloc int[buffer.Count(';') + 1];

        var currentCodeIndex = 0;
        for (var i = 0; i < buffer.Length; ++i)
        {
            if (buffer[i] is 'm' or ';')
            {
                ++currentCodeIndex;
                continue;
            }

            codes[currentCodeIndex] = codes[currentCodeIndex] * 10 + (buffer[i] - '0');
        }

        while (codes.Length > 0)
            codes = ApplySelectGraphicRendition(codes);

        buffer.Clear();
        return ParseState.Text;
    }

    private Span<int> ApplySelectGraphicRendition(Span<int> codes)
    {
        switch ((SelectGraphicRendition)codes[0])
        {
            case SelectGraphicRendition.ResetAll: _state.ResetGraphic(); break;
            case SelectGraphicRendition.Bold: _state.IsBold = true; break;
            case SelectGraphicRendition.Faint: _state.IsFaint = true; break;
            case SelectGraphicRendition.Italic: _state.IsItalic = true; break;
            case SelectGraphicRendition.Underline: _state.IsUnderline = true; break;
            case SelectGraphicRendition.Blink: _state.Blink = BlinkKind.Slow; break;
            case SelectGraphicRendition.FastBlink: _state.Blink = BlinkKind.Fast; break;
            case SelectGraphicRendition.StrikeThrough: _state.IsStrikeThrough = true; break;
            case SelectGraphicRendition.PrimaryFont: _state.FontFamily = _fontTable[FontKind.Primary]; break;
            case SelectGraphicRendition.AlternativeFont1: _state.FontFamily = _fontTable[FontKind.Alternative1]; break;
            case SelectGraphicRendition.AlternativeFont2: _state.FontFamily = _fontTable[FontKind.Alternative2]; break;
            case SelectGraphicRendition.AlternativeFont3: _state.FontFamily = _fontTable[FontKind.Alternative3]; break;
            case SelectGraphicRendition.AlternativeFont4: _state.FontFamily = _fontTable[FontKind.Alternative4]; break;
            case SelectGraphicRendition.AlternativeFont5: _state.FontFamily = _fontTable[FontKind.Alternative5]; break;
            case SelectGraphicRendition.AlternativeFont6: _state.FontFamily = _fontTable[FontKind.Alternative6]; break;
            case SelectGraphicRendition.AlternativeFont7: _state.FontFamily = _fontTable[FontKind.Alternative7]; break;
            case SelectGraphicRendition.AlternativeFont8: _state.FontFamily = _fontTable[FontKind.Alternative8]; break;
            case SelectGraphicRendition.AlternativeFont9: _state.FontFamily = _fontTable[FontKind.Alternative9]; break;
            case SelectGraphicRendition.ResetBold: _state.IsBold = false; break;
            case SelectGraphicRendition.ResetFaint: _state.IsFaint = false; break;
            case SelectGraphicRendition.ResetItalic: _state.IsItalic = false; break;
            case SelectGraphicRendition.ResetUnderline: _state.IsUnderline = false; break;
            case SelectGraphicRendition.ResetBlink: _state.Blink = BlinkKind.None; break;
            case SelectGraphicRendition.ResetStrikeThrough: _state.IsStrikeThrough = false; break;
            case SelectGraphicRendition.ForegroundBlack:
            case SelectGraphicRendition.ForegroundRed:
            case SelectGraphicRendition.ForegroundGreen:
            case SelectGraphicRendition.ForegroundYellow:
            case SelectGraphicRendition.ForegroundBlue:
            case SelectGraphicRendition.ForegroundMagenta:
            case SelectGraphicRendition.ForegroundCyan:
            case SelectGraphicRendition.ForegroundWhite:
                _state.DefaultForeground = false;
                _state.Foreground = _colorTable[codes[0] - (int)SelectGraphicRendition.ForegroundBlack];
                break;
            case SelectGraphicRendition.ForegroundCustom:
            {
                if (codes.Length < 2) return [];
                var kind = codes[1];
                switch (kind)
                {
                    case 5:
                    {
                        if (codes.Length < 3) return [];
                        _state.DefaultForeground = false;
                        _state.Foreground = _colorTable[codes[2]];
                        return codes[3..];
                    }

                    case 2:
                    {
                        if (codes.Length < 5) return [];
                        _state.DefaultForeground = false;
                        _state.Foreground = new Color((byte)codes[2], (byte)codes[3], (byte)codes[4]);
                        return codes[5..];
                    }
                }
                break;
            }
            case SelectGraphicRendition.DefaultForeground:
                _state.DefaultForeground = true;
                break;
            case SelectGraphicRendition.BackgroundBlack:
            case SelectGraphicRendition.BackgroundRed:
            case SelectGraphicRendition.BackgroundGreen:
            case SelectGraphicRendition.BackgroundYellow:
            case SelectGraphicRendition.BackgroundBlue:
            case SelectGraphicRendition.BackgroundMagenta:
            case SelectGraphicRendition.BackgroundCyan:
            case SelectGraphicRendition.BackgroundWhite:
                _state.DefaultBackground = false;
                _state.Background = _colorTable[codes[0] - (int)SelectGraphicRendition.BackgroundBlack];
                break;
            case SelectGraphicRendition.BackgroundCustom:
            {
                if (codes.Length < 2) return [];
                var kind = codes[1];
                switch (kind)
                {
                    case 5:
                    {
                        if (codes.Length < 3) return [];
                        _state.DefaultForeground = false;
                        _state.Foreground = _colorTable[codes[2]];
                        return codes[3..];
                    }

                    case 2:
                    {
                        if (codes.Length < 5) return [];
                        _state.DefaultForeground = false;
                        _state.Foreground = new Color((byte)codes[2], (byte)codes[3], (byte)codes[4]);
                        return codes[5..];
                    }
                }
                break;
            }
            case SelectGraphicRendition.DefaultBackground:
                _state.DefaultBackground = true;
                break;
            case SelectGraphicRendition.Overline: _state.IsOverline = true; break;
            case SelectGraphicRendition.ResetOverline: _state.IsOverline = false; break;
            case SelectGraphicRendition.Superscript: _state.SuperOrSubscript = SuperOrSubscript.Superscript; break;
            case SelectGraphicRendition.Subscript: _state.SuperOrSubscript = SuperOrSubscript.Subscript; break;
            case SelectGraphicRendition.ResetSuperOrSubscript: _state.SuperOrSubscript = SuperOrSubscript.Default; break;
            case SelectGraphicRendition.ForegroundHighIntensityBlack:
            case SelectGraphicRendition.ForegroundHighIntensityRed:
            case SelectGraphicRendition.ForegroundHighIntensityGreen:
            case SelectGraphicRendition.ForegroundHighIntensityYellow:
            case SelectGraphicRendition.ForegroundHighIntensityBlue:
            case SelectGraphicRendition.ForegroundHighIntensityMagenta:
            case SelectGraphicRendition.ForegroundHighIntensityCyan:
            case SelectGraphicRendition.ForegroundHighIntensityWhite:
                _state.DefaultForeground = false;
                _state.Foreground = _colorTable[codes[0] - (int)SelectGraphicRendition.ForegroundHighIntensityBlack + 8];
                break;
            case SelectGraphicRendition.BackgroundHighIntensityBlack:
            case SelectGraphicRendition.BackgroundHighIntensityRed:
            case SelectGraphicRendition.BackgroundHighIntensityGreen:
            case SelectGraphicRendition.BackgroundHighIntensityYellow:
            case SelectGraphicRendition.BackgroundHighIntensityBlue:
            case SelectGraphicRendition.BackgroundHighIntensityMagenta:
            case SelectGraphicRendition.BackgroundHighIntensityCyan:
            case SelectGraphicRendition.BackgroundHighIntensityWhite:
                _state.DefaultBackground = false;
                _state.Background = _colorTable[codes[0] - (int)SelectGraphicRendition.BackgroundHighIntensityBlack + 8];
                break;
        }
        
        return codes[1..];
    }

    private ParseState ParseOperatingSystemControl(TextReader reader, IStringBuilder buffer)
    {
        var code = reader.Read();
        if (code == -1) return ParseState.Eof;

        // Supported only Hyperlink
        if ((char)code is not '8')
            return SkipUntilStringTerminator(reader);

        if ((code = reader.Read()) != ';') return code == -1 ? ParseState.Eof : ParseState.Text;
        if ((code = reader.Read()) != ';') return code == -1 ? ParseState.Eof : ParseState.Text;
        
        var lastCh = (char)reader.Peek();
        int chInt;

        while ((chInt = reader.Read()) != -1)
        {
            var ch = (char)chInt;
            if (lastCh == '\e' && ch == '\x9c')
            {
                _state.HyperlinkUrl = buffer.ToString();
                buffer.Clear();
                return ParseState.Text;
            }

            if (ch != '\e')
                buffer.Append(ch);
            
            lastCh = ch;
        }
        
        return ParseState.Eof;
    }

    private ParseState ParsePrivacyMessage(TextReader reader, IStringBuilder builder, IStringBuilder buffer)
    {
        var lastCh = (char)reader.Peek();
        int chInt;

        while ((chInt = reader.Read()) != -1)
        {
            var ch = (char)chInt;
            if (lastCh == '\e' && ch == '\x9c')
            {
                stringifier.Stringify(builder, _state, buffer.AsSpan());
                buffer.Clear();
                return ParseState.Text;
            }

            if (ch != '\e')
                buffer.Append('*');
            
            lastCh = ch;
        }
        
        return ParseState.Eof;
    }
    
    private enum ParseState
    {
        Text,
        
        StartEscape,
        
        SelectGraphicRendition,
        OperatingSystemControl,
        PrivacyMessage,
        
        Eof,
    }
    
    private enum SelectGraphicRendition : byte
    {
        ResetAll = 0,
    
        Bold = 1,
        Faint = 2,
        Italic = 3,
        Underline = 4,
        Blink = 5,
        FastBlink = 6,
    
        StrikeThrough = 9,
        PrimaryFont = 10,
        AlternativeFont1 = 11,
        AlternativeFont2 = 12,
        AlternativeFont3 = 13,
        AlternativeFont4 = 14,
        AlternativeFont5 = 15,
        AlternativeFont6 = 16,
        AlternativeFont7 = 17,
        AlternativeFont8 = 18,
        AlternativeFont9 = 19,
    
        ResetBold = 21,
        ResetFaint = 22,
        ResetItalic = 23,
        ResetUnderline = 24,
        ResetBlink = 25,
        
        ResetStrikeThrough = 29,
        
        ForegroundBlack = 30,
        ForegroundRed = 31,
        ForegroundGreen = 32,
        ForegroundYellow = 33,
        ForegroundBlue = 34,
        ForegroundMagenta = 35,
        ForegroundCyan = 36,
        ForegroundWhite = 37,
        ForegroundCustom = 38,
        DefaultForeground = 39,
    
        BackgroundBlack = 40,
        BackgroundRed = 41,
        BackgroundGreen = 42,
        BackgroundYellow = 43,
        BackgroundBlue = 44,
        BackgroundMagenta = 45,
        BackgroundCyan = 46,
        BackgroundWhite = 47,
        BackgroundCustom = 48,
        DefaultBackground = 49,
    
        Overline = 53,
        ResetOverline = 55,
    
        Superscript = 73,
        Subscript = 74,
        ResetSuperOrSubscript = 75,
    
        ForegroundHighIntensityBlack = 90,
        ForegroundHighIntensityRed = 91,
        ForegroundHighIntensityGreen = 92,
        ForegroundHighIntensityYellow = 93,
        ForegroundHighIntensityBlue = 94,
        ForegroundHighIntensityMagenta = 95,
        ForegroundHighIntensityCyan = 96,
        ForegroundHighIntensityWhite = 97,
    
        BackgroundHighIntensityBlack = 100,
        BackgroundHighIntensityRed = 101,
        BackgroundHighIntensityGreen = 102,
        BackgroundHighIntensityYellow = 103,
        BackgroundHighIntensityBlue = 104,
        BackgroundHighIntensityMagenta = 105,
        BackgroundHighIntensityCyan = 106,
        BackgroundHighIntensityWhite = 107,
    }
}