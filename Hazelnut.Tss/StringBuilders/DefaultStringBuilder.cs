using System.Text;

namespace Hazelnut.Tss.StringBuilders;

public class DefaultStringBuilderFactory : IStringBuilderFactory
{
    public static DefaultStringBuilderFactory SharedInstance { get; } = new();
    
    public IStringBuilder Create() => new DefaultStringBuilder();
}

public class DefaultStringBuilder(StringBuilder builder) : IStringBuilder
{
    public static implicit operator DefaultStringBuilder(StringBuilder builder) => new(builder);

    public DefaultStringBuilder() : this(new()) { }

    ~DefaultStringBuilder() => Dispose();

    public void Dispose()
    {
        builder.Clear();
        GC.SuppressFinalize(this);
    }

    public int Length => builder.Length;

    public char this[int index]
    {
        get => builder[index];
        set => builder[index] = value;
    }

    public char this[Index index]
    {
        get => builder[index];
        set => builder[index] = value;
    }

    public int Count(char ch)
    {
        var count = 0;
        for (var i = 0; i < builder.Length; ++i)
            if (builder[i] == ch)
                ++count;
        return count;
    }

    public IStringBuilder Clear()
    {
        builder.Clear();
        return this;
    }

    public IStringBuilder Append(char ch)
    {
        builder.Append(ch);
        return this;
    }

    public IStringBuilder Append(string text)
    {
        builder.Append(text);
        return this;
    }

    public IStringBuilder Append(ReadOnlySpan<char> text)
    {
        builder.Append(text);
        return this;
    }

    public IStringBuilder Append<T>(T value)
    {
        builder.Append(value);
        return this;
    }

    public IStringBuilder AppendLine()
    {
        builder.AppendLine();
        return this;
    }

    public IStringBuilder AppendLine(char ch)
    {
        builder.AppendLine(ch.ToString());
        return this;
    }

    public IStringBuilder AppendLine(string text)
    {
        builder.AppendLine(text);
        return this;
    }

    public IStringBuilder AppendLine(ReadOnlySpan<char> text)
    {
        builder.AppendLine(new string(text));
        return this;
    }

    public IStringBuilder AppendLine<T>(T value)
    {
        if (value != null)
            builder.AppendLine(value.ToString());
        return this;
    }

    public IStringBuilder AppendFormat<T1>(string format, T1 arg1)
    {
        builder.AppendFormat(format, arg1);
        return this;
    }

    public IStringBuilder AppendFormat<T1, T2>(string format, T1 arg1, T2 arg2)
    {
        builder.AppendFormat(format, arg1, arg2);
        return this;
    }

    public IStringBuilder AppendFormat<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
    {
        builder.AppendFormat(format, arg1, arg2, arg3);
        return this;
    }

    public IStringBuilder AppendFormat<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        builder.AppendFormat(format, arg1, arg2, arg3, arg4);
        return this;
    }

    public IStringBuilder AppendFormat<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        builder.AppendFormat(format, arg1, arg2, arg3, arg4, arg5);
        return this;
    }

    public IStringBuilder
        AppendFormat<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        builder.AppendFormat(format, arg1, arg2, arg3, arg4, arg5, arg6);
        return this;
    }

    public IStringBuilder AppendFormat<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5,
        T6 arg6, T7 arg7)
    {
        builder.AppendFormat(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        return this;
    }

    public IStringBuilder AppendFormat<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5,
        T6 arg6, T7 arg7, T8 arg8)
    {
        builder.AppendFormat(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        return this;
    }

    public IStringBuilder AppendFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4,
        T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
        builder.AppendFormat(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        return this;
    }

    public IStringBuilder AppendFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4,
        T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
    {
        builder.AppendFormat(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        return this;
    }

    public override string ToString() => builder.ToString();

    public ReadOnlySpan<char> AsSpan()
    {
        return builder.ToString();
    }
}