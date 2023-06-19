using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using Bearz.Secrets;

namespace Bearz.Virtual;

#if NET5_0_OR_GREATER

public partial interface IVirtualEnvironment
{
    string? Get(string variableName)
        => Env.Get(variableName);

    bool Has(string variableName)
        => Env.Has(variableName);

    IDictionary<string, string> List()
    {
        var dictionary = new Dictionary<string, string>();
        var env = System.Environment.GetEnvironmentVariables();
        foreach (var key in env.Keys)
        {
            if (key is string name)
            {
                var value = env[name];
                if (value is string v)
                    dictionary[name] = v;
            }
        }

        return dictionary;
    }

    void Remove(string variableName)
        => Env.Remove(variableName);

    string Directory(KnownDirectory directory)
        => Env.Directory(directory);

    string Directory(KnownDirectory directory, KnownDirectoryOption option)
        => Env.Directory(directory, option);

    string Expand(string template, EnvExpandOptions? options = null)
        => Env.Expand(template, options);

    ReadOnlySpan<char> Expand(ReadOnlySpan<char> template, EnvExpandOptions? options = null)
        => Env.Expand(template, options);

    void Set(string variableName, string value)
        => Env.Set(variableName, value);

    void Set(string variableName, string value, bool secret)
    {
        if (secret)
            this.SecretMasker.Add(value);

        Env.Set(variableName, value);
    }

    bool TryGet(string variableName, [NotNullWhen(true)] out string? value)
        => Env.TryGet(variableName, out value);
}

#else

public partial interface IVirtualEnvironment
{
    string? Get(string variableName);

    bool Has(string variableName);

    IDictionary<string, string> List();

    void Remove(string variableName);

    string Directory(KnownDirectory directory);

    string Directory(KnownDirectory directory, KnownDirectoryOption option);

    string Expand(string template, EnvExpandOptions? options = null);

    ReadOnlySpan<char> Expand(ReadOnlySpan<char> template, EnvExpandOptions? options = null);

    void Set(string variableName, string value);

    void Set(string variableName, string value, bool secret);

    bool TryGet(string variableName, out string? value);
}

#endif