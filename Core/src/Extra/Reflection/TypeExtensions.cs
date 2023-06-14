namespace Bearz.Extra.Reflection;

public static class TypeExtensions
{
    public static bool IsPrimitive(this Type type)
    {
        if (type == typeof(string))
            return true;

        return type is { IsValueType: true, IsPrimitive: true };
    }
}