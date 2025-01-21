using System.Text;

namespace Hazelnut.Tss;

public interface IStringifier
{
    void Stringify(IStringBuilder output, in AnsiCodeState state, ReadOnlySpan<char> text);
    void Stringify(IStringBuilder output, in AnsiCodeState state, string text);

    void Escape(char ch, IStringBuilder buffer);
}