using System.Runtime.CompilerServices;

namespace Bearz.Modules;

#if STD
public
#else
internal
#endif
    static partial class Arrays
{
    /// <summary>
    /// Swaps the values of the two items in the array.
    /// </summary>
    /// <param name="array">The one dimensional array where values will be swapped.</param>
    /// <param name="index1">The first zero-based index where the first item exists.</param>
    /// <param name="index2">The second zero-based index where the second item exists.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Swap<T>(T[] array, int index1, int index2)
    {
        (array[index1], array[index2]) = (array[index2], array[index1]);
    }
}