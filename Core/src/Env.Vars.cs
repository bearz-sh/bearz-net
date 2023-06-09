using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Bearz;

public static partial class Env
{
    public static EnvVars Vars { get; } = new EnvVars();

    public static string? GetVar(string name)
    {
        return Environment.GetEnvironmentVariable(name);
    }

    public static string? GetVar(string name, EnvironmentVariableTarget target)
    {
        return Environment.GetEnvironmentVariable(name, target);
    }

    public static string GetRequiredVar(string name)
    {
        var value = Environment.GetEnvironmentVariable(name);
        if (value == null)
            throw new KeyNotFoundException($"Environment variable '{name}' not found.");

        return value;
    }

    public static bool HasVar(string name)
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

    public static void SetVar(string name, string value)
    {
        Environment.SetEnvironmentVariable(name, value);
    }

    public static void SetVar(string name, string value, EnvironmentVariableTarget target)
    {
        Environment.SetEnvironmentVariable(name, value, target);
    }

    public static bool TryGetVar(string name, [NotNullWhen(true)] out string? value)
    {
        value = Environment.GetEnvironmentVariable(name);
        return value != null;
    }

    public static IEnumerator<PathStr> SplitPath()
    {
        var name = IsWindows ? "Path" : "PATH";
        var path = GetVar(name) ?? string.Empty;
        foreach (var line in path.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries))
            yield return line;
    }

    public static IEnumerator<PathStr> SplitPath(string path)
    {
        foreach (var line in path.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries))
            yield return line;
    }

    public static string JoinPath(IEnumerable<PathStr> paths)
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
            get => Env.GetVar(name);
            set => Env.SetVar(name, value ?? string.Empty);
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