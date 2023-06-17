using System.Diagnostics.CodeAnalysis;

namespace Bearz.Extra.Strings;

#if STD
public
#else
internal
#endif
static partial class StringExtensions
{
    /// <summary>
    /// Indicates whether this string is equal to the given value using
    /// the <see cref="StringComparison.OrdinalIgnoreCase"/> comparison.
    /// </summary>
    /// <param name="source">The source string.</param>
    /// <param name="value">The value to test for equality.</param>
    /// <returns><see langword="true" /> when the string equals the value; otherwise, <see langword="false" />.</returns>
    public static bool EqualsIgnoreCase(this string? source, string? value)
    {
        if (ReferenceEquals(source, value))
            return true;

        return source?.Equals(value, StringComparison.OrdinalIgnoreCase) == true;
    }
}