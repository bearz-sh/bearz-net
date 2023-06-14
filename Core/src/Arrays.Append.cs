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
    /// Appends the item to the end of the array and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="element1">The first item to append.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>Returns the resized array.</returns>
    [Pure]
    public static T[] Append<T>(ref T[] array, T element1)
    {
        var copy = new T[array.Length + 1];
        Array.Copy(array, copy, array.Length);
        copy[^1] = element1;
        return array;
    }

    /// <summary>
    /// Appends two items to the end of the array and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="element1">The first item to append.</param>
    /// <param name="element2">The second item to append.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>Returns the resized array.</returns>
    [Pure]
    public static T[] Append<T>(ref T[] array, T element1, T element2)
    {
        var copy = new T[array.Length + 2];
        Array.Copy(array, copy, array.Length);
        copy[^2] = element1;
        copy[^1] = element2;
        return copy;
    }

    /// <summary>
    /// Appends three items to the end of the array and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="element1">The first item to append.</param>
    /// <param name="element2">The second item to append.</param>
    /// <param name="element3">The third item to append.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>Returns the resized array.</returns>
    [Pure]
    public static T[] Append<T>(T[] array, T element1, T element2, T element3)
    {
        var copy = new T[array.Length + 3];
        Array.Copy(array, copy, array.Length);
        copy[^3] = element1;
        copy[^2] = element2;
        copy[^1] = element3;
        return copy;
    }

    /// <summary>
    /// Appends elements to the end of the array and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="elements">The elements to append at the end of the array.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>Returns the resized array.</returns>
    [Pure]
    public static T[] Append<T>(T[] array, params T[] elements)
    {
        var copy = new T[array.Length + elements.Length];
        Array.Copy(array, copy, array.Length);
        Array.Copy(elements, 0, copy, array.Length, elements.Length);
        return copy;
    }

    /// <summary>
    /// Appends elements in a span to the end of the array and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="elements">The elements to append at the end of the array.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>Returns the resized array.</returns>
    [Pure]
    public static T[] Append<T>(T[] array, Span<T> elements)
    {
        var copy = new T[array.Length + elements.Length];
        Array.Copy(array, copy, array.Length);
        elements.CopyTo(copy.AsSpan(array.Length));
        return copy;
    }

    /// <summary>
    /// Appends elements in a readonly span to the end of the array and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="elements">The elements to append at the end of the array.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>Returns the resized array.</returns>
    [Pure]
    public static T[] Append<T>(ref T[] array, ReadOnlySpan<T> elements)
    {
        var copy = new T[array.Length + elements.Length];
        Array.Copy(array, copy, array.Length);
        elements.CopyTo(copy.AsSpan(array.Length));
        return copy;
    }

    /// <summary>
    /// Appends elements in an enumerable object to the end of the array and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="elements">The elements to append at the end of the array.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>Returns the resized array.</returns>
    [Pure]
    public static T[] Append<T>(T[] array, IEnumerable<T> elements)
    {
        switch (elements)
        {
            case T[] array1:
                return Append(array, array1.AsSpan());

            case List<T> list:
                {
                    var copy = new T[array.Length + list.Count];
                    Array.Copy(array, copy, array.Length);
                    list.CopyTo(copy, array.Length);
                    return copy;
                }

            case IReadOnlyCollection<T> readOnlyCollection:
                {
                    var copy = new T[array.Length + readOnlyCollection.Count];
                    Array.Copy(array, copy, array.Length);
                    var j = array.Length;
                    foreach (var item in readOnlyCollection)
                        copy[j++] = item;

                    return copy;
                }

            default:
#if !NETLEGACY
                if (elements.TryGetNonEnumeratedCount(out var count))
                {
                    var copy = new T[array.Length + count];
                    Array.Copy(array, copy, array.Length);
                    var j = array.Length;
                    foreach (var item in elements)
                        copy[j++] = item;

                    return copy;
                }
#endif
                return Append(array, elements.ToArray().AsSpan());
        }
    }
}