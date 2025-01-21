using System.Text;

namespace Hazelnut.Tss.Stringifiers;

public class PlainTextStringifier : IStringifier
{
    public static IStringifier SharedInstance { get; } = new PlainTextStringifier();
    
    private PlainTextStringifier() { }

    public void Stringify(IStringBuilder output, in AnsiCodeState state, ReadOnlySpan<char> text) =>
        output.Append(text);
    public void Stringify(IStringBuilder output, in AnsiCodeState state, string text) =>
        output.Append(text);
    
    public void Escape(char ch, IStringBuilder buffer) => buffer.Append(ch);
}