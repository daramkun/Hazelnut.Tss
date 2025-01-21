using System.Text;

namespace Hazelnut.Tss.Stringifiers;

public class HtmlStringifier : IStringifier
{
    public static IStringifier SharedInstance { get; } = new HtmlStringifier();

    public void Stringify(IStringBuilder output, in AnsiCodeState state, string text) =>
        Stringify(output, state, text.AsSpan());
    
    public void Stringify(IStringBuilder output, in AnsiCodeState state, ReadOnlySpan<char> text)
    {
        if (!string.IsNullOrEmpty(state.HyperlinkUrl))
            output.Append("<a href=\"").Append(state.HyperlinkUrl).Append("\">");

        if (state.IsDefault)
            output.Append(text);
        else
        {
            output.Append("<span style=\"");
            if (state.IsFaint) output.Append("opacity: 0.7;");
            if (state.IsBold) output.Append("text-weight: bold;");
            if (state.IsItalic) output.Append("text-style: italic;");
            switch (state.SuperOrSubscript)
            {
                case SuperOrSubscript.Superscript: output.Append("vertical-align: super;"); break;
                case SuperOrSubscript.Subscript: output.Append("vertical-align: sub;"); break;
            }

            switch (state.Blink)
            {
                case BlinkKind.Slow: output.Append("animation: blink 1s infinite;"); break;
                case BlinkKind.Fast: output.Append("animation: blink 0.5s infinite;"); break;
            }

            if (state is { IsUnderline: true, IsStrikeThrough: true, IsOverline: true })
                output.Append("text-decoration: underline line-through overline;");
            else if (state is { IsUnderline: true, IsStrikeThrough: true, IsOverline: false })
                output.Append("text-decoration: underline line-through;");
            else if (state is { IsUnderline: true, IsStrikeThrough: false, IsOverline: true })
                output.Append("text-decoration: underline overline;");
            else if (state is { IsUnderline: false, IsStrikeThrough: true, IsOverline: true })
                output.Append("text-decoration: line-through overline;");
            else if (state is { IsUnderline: true, IsStrikeThrough: false, IsOverline: false })
                output.Append("text-decoration: underline;");
            else if (state is { IsUnderline: false, IsStrikeThrough: true, IsOverline: false })
                output.Append("text-decoration: line-through;");
            else if (state is { IsUnderline: false, IsStrikeThrough: false, IsOverline: true })
                output.Append("text-decoration: overline;");
            if (!state.DefaultForeground)
                output.AppendFormat("color: #{0:X2}{1:X2}{2:X2};",
                    state.Foreground.Red, state.Foreground.Green, state.Foreground.Blue);
            if (!state.DefaultBackground)
                output.AppendFormat("background-color: #{0:X2}{1:X2}{2:X2};",
                    state.Background.Red, state.Background.Green, state.Background.Blue);
            if (!string.IsNullOrEmpty(state.FontFamily))
                output.Append("font-family: ").Append(state.FontFamily).Append(';');

            output.Append("\">").Append(text).Append("</span>");
        }

        if (!string.IsNullOrEmpty(state.HyperlinkUrl))
            output.Append("</a>");
    }
    
    public void Escape(char ch, IStringBuilder buffer)
    {
        switch (ch)
        {
            case '<': buffer.Append("&lt;"); break;
            case '>': buffer.Append("&gt;"); break;
            case '&': buffer.Append("&amp;"); break;
            case '"': buffer.Append("&quot;"); break;
            case '\'': buffer.Append("&apos;"); break;
            case ' ': buffer.Append("&nbsp;"); break;
            case '\t': buffer.Append("&#9;"); break;
            default: buffer.Append(ch); break;
        }
    }
}