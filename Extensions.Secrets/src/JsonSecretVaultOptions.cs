using System.Text;
using System.Text.Json;

using Bearz.Security.Cryptography;

namespace Bearz.Extensions.Secrets;

public class JsonSecretVaultOptions : SecretVaultOptions
{
    public string Path { get; set; } = string.Empty;

    public byte[] Key { get; set; } = Array.Empty<byte>();

    public IEncryptionProvider? EncryptionProvider { get; set; }

    public Encoding Encoding { get; set; } = new UTF8Encoding(false, true);

    public JsonSerializerOptions JsonSerializerOptions { get; set; } = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        IgnoreReadOnlyProperties = true,
        AllowTrailingCommas = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        PropertyNameCaseInsensitive = true,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
    };
}