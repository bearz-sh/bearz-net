using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

using HandlebarsDotNet;

namespace Bearz.Handlebars.Helpers;

public static class JsonHelpers
{
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically ed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static void ConvertToJson(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "json");
        var value = arguments[0];
        if (value is null)
        {
            return;
        }

        var json = JsonSerializer.Serialize(value, new JsonSerializerOptions()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        writer.WriteSafeString(json);
    }

    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically ed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static void RegisterJsonHelpers(this IHandlebars? hb)
    {
        if (hb is null)
        {
            HandlebarsDotNet.Handlebars.RegisterHelper("json", ConvertToJson);

            return;
        }

        hb.RegisterHelper("json", ConvertToJson);
    }
}