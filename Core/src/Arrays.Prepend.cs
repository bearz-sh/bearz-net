namespace Bearz;

#if STD
public
#else
internal
#endif
    static partial class Arrays
{
    /// <summary>
    /// Prepends an element to the array and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="element1">The first element to prepend.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>The resized array.</returns>
    public static T[] Prepend<T>(T[] array, T element1)
    {
        var copy = new T[array.Length + 1];
        Array.Copy(array, 0, copy, 1, array.Length);
        copy[0] = element1;
        return copy;
    }

    /// <summary>
    /// Prepends two elements to the array and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="element1">The first element to prepend.</param>
    /// <param name="element2">The second element to prepend.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>The resized array.</returns>
    public static T[] Prepend<T>(T[] array, T element1, T element2)
    {
        var copy = new T[array.Length + 2];
        Array.Copy(array, 0, copy, 2, array.Length);
        copy[0] = element1;
        copy[1] = element2;
        return copy;
    }

    /// <summary>
    /// Prepends three elements to the array and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="element1">The first element to prepend.</param>
    /// <param name="element2">The second element to prepend.</param>
    /// <param name="element3">The third element to prepend.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>The resized array.</returns>
    public static T[] Prepend<T>(T[] array, T element1, T element2, T element3)
    {
        var copy = new T[array.Length + 3];
        Array.Copy(array, 0, copy, 3, array.Length);
        copy[0] = element1;
        copy[1] = element2;
        copy[2] = element3;
        return copy;
    }

    /// <summary>
    /// Prepends elements to the array and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="elements">The elements to prepend.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>The resized array.</returns>
    public static T[] Prepend<T>(T[] array, params T[] elements)
    {
        var copy = new T[array.Length + elements.Length];
        Array.Copy(array, 0, copy, elements.Length, array.Length);
        Array.Copy(elements, copy, elements.Length);
        return copy;
    }

    /// <summary>
    /// Prepends a span of elements to the array and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="elements">The elements to prepend.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>The resized array.</returns>
    public static T[] Prepend<T>(T[] array, Span<T> elements)
    {
        var copy = new T[array.Length + elements.Length];
        Array.Copy(array, 0, copy, elements.Length, array.Length);
        elements.CopyTo(copy);
        return copy;
    }

    /// <summary>
    /// Prepends a readonly span of elements to the array and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="elements">The elements to prepend.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>The resized array.</returns>
    public static T[] Prepend<T>(T[] array, ReadOnlySpan<T> elements)
    {
        var copy = new T[array.Length + elements.Length];
        Array.Copy(array, 0, copy, elements.Length, array.Length);
        elements.CopyTo(copy);
        return copy;
    }

    /// <summary>
    /// Prepends an enumerable of elements to the array and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="elements">The elements to prepend.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>The resized array.</returns>
    public static T[] Prepend<T>(T[] array, IEnumerable<T> elements)
    {
        switch (elements)
        {
            case T[] array1:
                return Prepend(array, array1);

            case List<T> list:
                {
                    var copy = new T[array.Length + list.Count];
                    Array.Copy(array, 0, copy, list.Count, array.Length);
                    list.CopyTo(copy);
                    return copy;
                }

            case IReadOnlyCollection<T> readOnlyCollection:
                {
                    var copy = new T[array.Length + readOnlyCollection.Count];
                    Array.Copy(array, 0, copy, readOnlyCollection.Count, array.Length);
                    var j = 0;
                    foreach (var item in readOnlyCollection)
                        copy[j++] = item;

                    return copy;
                }

            default:
                return Prepend(array, elements.ToArray());
        }
    }
}