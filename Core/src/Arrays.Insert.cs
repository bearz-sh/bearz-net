using System.Diagnostics.Contracts;

namespace Bearz;

#if STD
public
#else
internal
#endif
    static partial class Arrays
{
    /// <summary>
    /// Inserts an element into the array at the specified index and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="index">The zero-based position to start the insertion.</param>
    /// <param name="element1">The first element to append.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>Returns the resized array.</returns>
    public static T[] Insert<T>(T[] array, int index, T element1)
    {
        var copy = new T[array.Length + 1];
        Array.Copy(array, 0, copy, 0, index);
        copy[index] = element1;
        Array.Copy(array, index, copy, index + 1, array.Length - index);
        return copy;
    }

    /// <summary>
    /// Inserts two elements into the array at the specified index and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="index">The zero-based position to start the insertion.</param>
    /// <param name="element1">The first element to append.</param>
    /// <param name="element2">The second element to append.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>Returns the resized array.</returns>
    public static T[] Insert<T>(T[] array, int index, T element1, T element2)
    {
        var copy = new T[array.Length + 2];
        Array.Copy(array, 0, copy, 0, index);
        copy[index] = element1;
        copy[index + 1] = element2;
        Array.Copy(array, index, copy, index + 2, array.Length - index);
        return copy;
    }

    /// <summary>
    /// Inserts three elements into the array at the specified index and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="index">The zero-based position to start the insertion.</param>
    /// <param name="element1">The first element to append.</param>
    /// <param name="element2">The second element to append.</param>
    /// <param name="element3">The third element to append.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>Returns the resized array.</returns>
    public static T[] Insert<T>(T[] array, int index, T element1, T element2, T element3)
    {
        var copy = new T[array.Length + 3];
        Array.Copy(array, 0, copy, 0, index);
        copy[index] = element1;
        copy[index + 1] = element2;
        copy[index + 2] = element3;
        Array.Copy(array, index, copy, index + 3, array.Length - index);
        return copy;
    }

    /// <summary>
    /// Inserts elements into the array at the specified index and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="index">The zero-based position to start the insertion.</param>
    /// <param name="elements">The elements to be inserted.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>Returns the resized array.</returns>
    public static T[] Insert<T>(T[] array, int index, params T[] elements)
    {
        var copy = new T[array.Length + elements.Length];
        Array.Copy(array, 0, copy, 0, index);
        Array.Copy(elements, 0, copy, index, elements.Length);
        Array.Copy(array, index, copy, index + elements.Length, array.Length - index);
        return copy;
    }

    /// <summary>
    /// Inserts a span of elements into the array at the specified index and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="index">The zero-based position to start the insertion.</param>
    /// <param name="elements">The elements to be inserted.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>Returns the resized array.</returns>
    public static T[] Insert<T>(ref T[] array, int index, Span<T> elements)
    {
        var copy = new T[array.Length + elements.Length];
        Array.Copy(array, 0, copy, 0, index);
        elements.CopyTo(copy.AsSpan(index));
        Array.Copy(array, index, copy, index + elements.Length, array.Length - index);
        return copy;
    }

    /// <summary>
    /// Inserts a readonly span of elements into the array at the specified index and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="index">The zero-based position to start the insertion.</param>
    /// <param name="elements">The elements to be inserted.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>Returns the resized array.</returns>
    public static T[] Insert<T>(T[] array, int index, ReadOnlySpan<T> elements)
    {
        var copy = new T[array.Length + elements.Length];
        Array.Copy(array, 0, copy, 0, index);
        elements.CopyTo(copy.AsSpan(index));
        Array.Copy(array, index, copy, index + elements.Length, array.Length - index);
        return copy;
    }

    /// <summary>
    /// Inserts an enumerable of elements into the array at the specified index and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="index">The zero-based position to start the insertion.</param>
    /// <param name="elements">The elements to be inserted.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>Returns the resized array.</returns>
    public static T[] Insert<T>(T[] array, int index, IEnumerable<T> elements)
    {
        switch (elements)
        {
            case T[] array1:
                return Insert(ref array, index, array1);

            case IList<T> list:
                {
                    var copy = new T[array.Length + list.Count];
                    Array.Copy(array, 0, copy, 0, index);
                    list.CopyTo(copy, index);
                    Array.Copy(array, index, copy, index + list.Count, array.Length - index);
                    return copy;
                }

            case IReadOnlyCollection<T> readOnlyCollection:
                {
                    var copy = new T[array.Length + readOnlyCollection.Count];
                    Array.Copy(array, 0, copy, 0, index);
                    var j = index;
                    foreach (var item in readOnlyCollection)
                        copy[j++] = item;

                    Array.Copy(array, index, copy, index + readOnlyCollection.Count, array.Length - index);
                    return copy;
                }

            default:
#if !NETLEGACY
                if (elements.TryGetNonEnumeratedCount(out var count))
                {
                    var copy = new T[array.Length + count];
                    Array.Copy(array, 0, copy, 0, index);
                    var j = index;
                    foreach (var item in elements)
                        copy[j++] = item;

                    Array.Copy(array, index, copy, index + count, array.Length - index);
                    return copy;
                }
#endif

                return Insert(array, index, elements.ToArray());
        }
    }
}