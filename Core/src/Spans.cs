using System.Text;

namespace Bearz;

public static class Spans
{
    /// <summary>
    /// Converts a span of characters to a string, including older versions of .NET.
    /// </summary>
    /// <param name="buffer">The buffer to convert.</param>
    /// <returns>A string of characters.</returns>
    public static string ConvertToString(ReadOnlySpan<char> buffer)
    {
#if NETLEGACY
        return new string(buffer.ToArray());
#else
        return buffer.ToString();
#endif
    }

    /// <summary>
    /// Converts a span of bytes to a string, including older versions of .NET.
    /// </summary>
    /// <param name="buffer">The buffer to convert.</param>
    /// <param name="encoding">The encoding to use.</param>
    /// <returns>A string of characters.</returns>
    public static string ConvertToString(ReadOnlySpan<byte> buffer, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
#if NETLEGACY
        return encoding.GetString(buffer.ToArray());
#else
        return encoding.GetString(buffer);
#endif
    }

    /// <summary>
    /// Shifts the first item from a span and returns the first item and updates the
    /// span reference with the item removed using slice. <c>ref></c> is used to update the span.
    /// </summary>
    /// <param name="span">The span of objects.</param>
    /// <typeparam name="T">The type of object in the span.</typeparam>
    /// <returns>The updated span with one less item.</returns>
    /// <exception cref="ArgumentException">Throws when the span is empty.</exception>
    public static T Shift<T>(ref Span<T> span)
    {
        if (span.IsEmpty)
            throw new ArgumentException("Cannot shift the first item from an empty span.");

        var value = span[0];
        span = span[1..];
        return value;
    }

    /// <summary>
    /// Shifts the first item from a readonly span and returns the first item and updates the
    /// span reference with the item removed using slice. <c>ref></c> is used to update the span.
    /// </summary>
    /// <param name="span">The span of objects.</param>
    /// <typeparam name="T">The type of object in the span.</typeparam>
    /// <returns>The updated span with one less item.</returns>
    /// <exception cref="ArgumentException">Throws when the span is empty.</exception>
    public static T Shift<T>(ref ReadOnlySpan<T> span)
    {
        if (span.IsEmpty)
            throw new ArgumentException("Cannot shift the first item from an empty span.");

        var value = span[0];
        span = span[1..];
        return value;
    }
}