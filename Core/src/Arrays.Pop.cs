using System.Diagnostics.CodeAnalysis;

namespace Bearz;

#if STD
public
#else
internal
#endif
    static partial class Arrays
{
    /// <summary>
    /// Pops the last item in the array, returns the item and a resized array.
    /// </summary>
    /// <param name="array">The array to resize.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>The last item in the array.</returns>
    /// <exception cref="ArgumentException">Throws when the length of the array is zero.</exception>
    public static (T, T[]) Pop<T>(T[] array)
    {
        if (array.Length == 0)
            throw new ArgumentException("Cannot pop the last item from an empty array.");

        var item = array[^1];
        var copy = new T[array.Length - 1];
        Array.Copy(array, copy, array.Length - 1);
        return (item, array);
    }

    /// <summary>
    /// Attempts to pop the last item from the end of the array and resize the array.
    /// </summary>
    /// <param name="array">The one dimensional array.</param>
    /// <param name="element">The last item of the array, if available.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns><c>True</c> when an element is available; otherwise, <c>false</c>.</returns>
    public static bool TryPop<T>(ref T[] array, [NotNullWhen(true)] out T? element)
    {
        if (array.Length == 0)
        {
            element = default;
            return false;
        }

        element = array[^1];
        var copy = new T[array.Length - 1];
        Array.Copy(array, copy, array.Length - 1);
        array = copy;
        return element is not null;
    }
}