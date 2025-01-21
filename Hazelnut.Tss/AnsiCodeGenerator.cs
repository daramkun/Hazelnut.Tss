using Hazelnut.Tss.StringBuilders;

namespace Hazelnut.Tss;

public class AnsiCodeGenerator(IStringBuilder builder, bool leaveOpen = false) : IDisposable
{
    public AnsiCodeGenerator() : this(new DefaultStringBuilder()) { }
    public AnsiCodeGenerator(IStringBuilderFactory factory) : this(factory.Create()) { }

    public void Dispose()
    {
        if (!leaveOpen)
            builder.Dispose();
    }

    public override string ToString() => builder.ToString();

    public AnsiCodeGenerator Clear()
    {
        builder.Clear();
        return this;
    }
    
    public AnsiCodeGenerator Append(ReadOnlySpan<char> text)
    {
        builder.Append(text);
        return this;
    }

    public AnsiCodeGenerator Append(IStringBuilder stringBuilder)
    {
        builder.Append(stringBuilder);
        return this;
    }

    public AnsiCodeGenerator Reset()
    {
        builder.Append("\e[0m");
        return this;
    }

    public AnsiCodeGenerator SetBold(bool enable = true)
    {
        builder.Append(enable ? "\e[1m" : "\e[21m");
        return this;
    }

    public AnsiCodeGenerator SetFaint(bool enable = true)
    {
        builder.Append(enable ? "\e[2m" : "\e[22m");
        return this;
    }

    public AnsiCodeGenerator SetItalic(bool enable = true)
    {
        builder.Append(enable ? "\e[3m" : "\e[23m");
        return this;
    }

    public AnsiCodeGenerator SetUnderline(bool enable = true)
    {
        builder.Append(enable ? "\e[4m" : "\e[24m");
        return this;
    }

    public AnsiCodeGenerator SetBlink(BlinkKind kind = BlinkKind.Slow)
    {
        builder.Append(kind switch
        {
            BlinkKind.Slow => "\e[5m",
            BlinkKind.Fast => "\e[6m",
            BlinkKind.None => "\e[25m",
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
        });
        return this;
    }

    public AnsiCodeGenerator SetStrikeThrough(bool enable = true)
    {
        builder.Append(enable ? "\e[9m" : "\e[29m");
        return this;
    }

    public AnsiCodeGenerator SetOverline(bool enable = true)
    {
        builder.Append(enable ? "\e[53m" : "\e[55m");
        return this;
    }

    public AnsiCodeGenerator SetSuperOrSubscript(SuperOrSubscript superOrSubscript = SuperOrSubscript.Default)
    {
        builder.Append(superOrSubscript switch
        {
            SuperOrSubscript.Superscript => "\e[73m",
            SuperOrSubscript.Subscript => "\e[74m",
            SuperOrSubscript.Default => "\e[75m",
            _ => throw new ArgumentOutOfRangeException(nameof(superOrSubscript), superOrSubscript, null)
        });
        return this;
    }

    public AnsiCodeGenerator ResetForeground()
    {
        builder.Append("\e[39m");
        return this;
    }

    public AnsiCodeGenerator SetForeground(AnsiColorCode color)
    {
        builder.Append("\e[").Append(color + (color <= AnsiColorCode.White ? 30 : 90)).Append('m');
        return this;
    }

    public AnsiCodeGenerator SetForeground(byte index)
    {
        builder.Append("\e[38;5;").Append(index).Append('m');
        return this;
    }

    public AnsiCodeGenerator SetForeground(Color color)
    {
        builder.Append("\e[38;2;")
            .Append(color.Red).Append(';')
            .Append(color.Green).Append(';')
            .Append(color.Blue).Append('m');
        return this;
    }

    public AnsiCodeGenerator ResetBackground()
    {
        builder.Append("\e[49m");
        return this;
    }

    public AnsiCodeGenerator SetBackground(AnsiColorCode color)
    {
        builder.Append("\e[").Append(color + (color <= AnsiColorCode.White ? 40 : 100)).Append('m');
        return this;
    }

    public AnsiCodeGenerator SetBackground(byte index)
    {
        builder.Append("\e[48;5;").Append(index).Append('m');
        return this;
    }

    public AnsiCodeGenerator SetBackground(Color color)
    {
        builder.Append("\e[48;2;")
            .Append(color.Red).Append(';')
            .Append(color.Green).Append(';')
            .Append(color.Blue).Append('m');
        return this;
    }
}