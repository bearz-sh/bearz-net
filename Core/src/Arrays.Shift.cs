using System.Diagnostics.CodeAnalysis;

namespace Bearz.Modules;

#if STD
public
#else
internal
#endif
    static partial class Arrays
{
    /// <summary>
    /// Shifts the first item from the array and returns it and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>Returns the last item in the array and the resized array.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the array is empty.
    /// </exception>
    public static (T, T[]) Shift<T>(T[] array)
    {
        if (array.Length == 0)
            throw new ArgumentException("Cannot shift the first item from an empty array.");

        var item = array[0];
        var copy = new T[array.Length - 1];
        Array.Copy(array, 1, copy, 0, array.Length - 1);
        return (item, copy);
    }

    /// <summary>
    /// Attempts to shift the first item from the beginning of the array and resize the array.
    /// </summary>
    /// <param name="array">The one dimensional array.</param>
    /// <param name="element">The last item of the array, if available.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns><c>True</c> when an element is available; otherwise, <c>false</c>.</returns>
    public static bool TryShift<T>(ref T[] array, [NotNullWhen(true)] out T? element)
    {
        if (array.Length == 0)
        {
            element = default;
            return false;
        }

        element = array[0];
        var copy = new T[array.Length - 1];
        Array.Copy(array, 1, copy, 0, array.Length - 1);
        array = copy;
        return element is not null;
    }
}