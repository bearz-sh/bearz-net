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
    /// Retrieves a one dimensional array from the shared pool.
    /// </summary>
    /// <param name="minimumLength">The minimum length of the array.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>The rented one dimensional array from the shared poo.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the <paramref name="minimumLength"/> is less than zero.
    /// </exception>
    public static T[] Rent<T>(int minimumLength)
    {
        if (minimumLength < 0)
            throw new ArgumentOutOfRangeException(nameof(minimumLength));

        return ArrayPool<T>.Shared.Rent(minimumLength);
    }
}