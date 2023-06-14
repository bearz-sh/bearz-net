namespace Bearz;

#if STD
public
#else
internal
#endif
static partial class Arrays
{
    /// <summary>
    /// Concatenates the two arrays into a single new array.
    /// </summary>
    /// <param name="array1">The first array to concatenate.</param>
    /// <param name="array2">The second array to concatenate.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>A new array.</returns>
    public static T[] Concat<T>(T[] array1, T[] array2)
    {
        var result = new T[array1.Length + array2.Length];
        Array.Copy(array1, result, array1.Length);
        Array.Copy(array2, 0, result, array1.Length, array2.Length);
        return result;
    }

    /// <summary>
    /// Concatenates the two arrays into a single new array.
    /// </summary>
    /// <param name="array1">The first array to concatenate.</param>
    /// <param name="array2">The second array to concatenate.</param>
    /// <param name="array3">The third array to concatenate.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>A new array.</returns>
    public static T[] Concat<T>(T[] array1, T[] array2, T[] array3)
    {
        var result = new T[array1.Length + array2.Length + array3.Length];
        Array.Copy(array1, result, array1.Length);
        Array.Copy(array2, 0, result, array1.Length, array2.Length);
        Array.Copy(array3, 0, result, array1.Length + array2.Length, array3.Length);
        return result;
    }

    /// <summary>
    /// Concatenates multiple arrays into a single new array.
    /// </summary>
    /// <param name="arrays">The array of arrays to concatenate.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>A new array.</returns>
    public static T[] Concat<T>(params T[][] arrays)
    {
        var result = new T[arrays.Sum(a => a.Length)];
        var offset = 0;
        foreach (var array in arrays)
        {
            Array.Copy(array, 0, result, offset, array.Length);
            offset += array.Length;
        }

        return result;
    }
}