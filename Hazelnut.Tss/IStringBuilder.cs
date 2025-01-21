namespace Hazelnut.Tss;

public interface IStringBuilderFactory
{
    IStringBuilder Create();
}

public interface IStringBuilder : IDisposable
{
    int Length { get; }
    
    char this[int index] { get; set; }
    char this[Index index] { get; set; }

    int Count(char ch);

    IStringBuilder Clear();

    IStringBuilder Append(char ch);
    IStringBuilder Append(string text);
    IStringBuilder Append(ReadOnlySpan<char> text);
    IStringBuilder Append<T>(T value);

    IStringBuilder AppendLine();
    IStringBuilder AppendLine(char ch);
    IStringBuilder AppendLine(string text);
    IStringBuilder AppendLine(ReadOnlySpan<char> text);
    IStringBuilder AppendLine<T>(T value);

    IStringBuilder AppendFormat<T1>(string format, T1 arg1);
    IStringBuilder AppendFormat<T1, T2>(string format, T1 arg1, T2 arg2);
    IStringBuilder AppendFormat<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3);
    IStringBuilder AppendFormat<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    IStringBuilder AppendFormat<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

    IStringBuilder AppendFormat<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5,
        T6 arg6);

    IStringBuilder AppendFormat<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5,
        T6 arg6, T7 arg7);

    IStringBuilder AppendFormat<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4,
        T5 arg5, T6 arg6, T7 arg7, T8 arg8);

    IStringBuilder AppendFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4,
        T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);

    IStringBuilder AppendFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3,
        T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);

    ReadOnlySpan<char> AsSpan();
    string ToString();
}