using System.Buffers;

namespace Bearz;

#if STD
public
#else
internal
#endif
    static partial class Arrays
{
    /// <summary>
    /// Returns a one dimensional rented array returned from <see cref="Rent{T}"/> method to the
    /// shared pool.
    /// </summary>
    /// <param name="array">A rented array to return to the shared pool.</param>
    /// <param name="clearArray">
    /// Instructs the pool to clear the contents of the array.
    /// When the contents of an array are not cleared, the contents are available
    /// when the array is rented again.
    /// </param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the array is null.
    /// </exception>
    public static void Return<T>(T[] array, bool clearArray = false)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));

        ArrayPool<T>.Shared.Return(array, clearArray);
    }
}