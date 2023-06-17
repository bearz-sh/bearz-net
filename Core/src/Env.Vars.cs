using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Bearz.Extra.Strings;

namespace Bearz;

public static partial class Env
{
    public static EnvVars Vars { get; } = new EnvVars();

    public static string? Get(string name)
    {
        return Environment.GetEnvironmentVariable(name);
    }

    public static string? Get(string name, EnvironmentVariableTarget target)
    {
        return Environment.GetEnvironmentVariable(name, target);
    }

    public static string GetRequired(string name)
    {
        var value = Environment.GetEnvironmentVariable(name);
        if (value == null)
            throw new KeyNotFoundException($"Environment variable '{name}' not found.");

        return value;
    }

    public static bool Has(string name)
    {
        return Environment.GetEnvironmentVariable(name) != null;
    }

    public static void RemoveVar(string name)
    {
        Environment.SetEnvironmentVariable(name, null);
    }

    public static void RemoveVar(string name, EnvironmentVariableTarget target)
    {
        Environment.SetEnvironmentVariable(name, null, target);
    }

    public static void Set(string name, string value)
    {
        Environment.SetEnvironmentVariable(name, value);
    }

    public static void Set(string name, string value, EnvironmentVariableTarget target)
    {
        Environment.SetEnvironmentVariable(name, value, target);
    }

    public static bool TryGetVar(string name, [NotNullWhen(true)] out string? value)
    {
        value = Environment.GetEnvironmentVariable(name);
        return value != null;
    }

    public static IEnumerable<string> SplitPath()
    {
        var name = IsWindows ? "Path" : "PATH";
        var path = Get(name) ?? string.Empty;
        return path.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);
    }

    public static IEnumerable<string> SplitPath(string path)
        => path.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);

    public static string JoinPath(IEnumerable<string> paths)
    {
        var sb = new StringBuilder();
        foreach (var path in paths)
        {
            if (sb.Length > 0)
                sb.Append(Path.PathSeparator);

            sb.Append(path.ToArray());
        }

        return sb.ToString();
    }

    public sealed class EnvVars : IEnumerable<KeyValuePair<string, string>>
    {
        public string? this[string name]
        {
            get => Env.Get(name);
            set => Env.Set(name, value ?? string.Empty);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach (DictionaryEntry entry in System.Environment.GetEnvironmentVariables())
            {
                if (entry.Value is null)
                    continue;

                yield return new KeyValuePair<string, string>((string)entry.Key, (string)entry.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}