using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Bearz;

public static class Types
{
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPrimitive(Type type)
    {
        if (type == typeof(string))
            return true;

        return type is { IsValueType: true, IsPrimitive: true };
    }
}