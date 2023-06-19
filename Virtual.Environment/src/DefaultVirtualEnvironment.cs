using System.Collections;
using System.Runtime.InteropServices;

using Bearz.Extra.Strings;
using Bearz.Secrets;

namespace Bearz.Virtual.Environment;

public class DefaultVirtualEnvironment : IVirtualEnvironment
{
    private readonly IVirtualEnvironmentPath path;

    private readonly ISecretMasker masker;

    private bool isUserInteractive;

    public DefaultVirtualEnvironment(ISecretMasker? masker = null)
    {
        this.masker = masker ?? Secrets.SecretMasker.Default;
        this.path = new DefaultVirtualEnvironmentPath();
    }

    public string Cwd
    {
        get => System.Environment.CurrentDirectory;
        set => System.Environment.CurrentDirectory = value;
    }

    public bool IsWindows => Env.IsWindows;

    public bool IsLinux => Env.IsLinux;

    public bool IsMacOS => Env.IsMacOS;

    public bool Is64BitProcess => Env.Is64BitProcess;

    public bool Is64BitOs => Env.Is64BitOs;

    public bool IsUserInteractive
    {
        get
        {
            if (System.Environment.UserInteractive)
                return this.isUserInteractive;

            return false;
        }
        set => this.isUserInteractive = value;
    }

    public bool IsPrivilegedProcess => Env.IsPrivilegedProcess;

    public bool IsWsl => Env.IsWsl;

    public Architecture OsArch => RuntimeInformation.OSArchitecture;

    public Architecture ProcessArch => RuntimeInformation.ProcessArchitecture;

    public IVirtualEnvironmentPath Path => this.path;

    public ISecretMasker SecretMasker => this.masker;

    public string User => Env.User;

    public string UserDomain => Env.UserDomain;

    public string Host => Env.Host;

    public string? this[string key]
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

#if !NET5_0_OR_GREATER
    public string? Get(string variableName)
        => System.Environment.GetEnvironmentVariable(variableName);

    public bool Has(string variableName)
        => System.Environment.GetEnvironmentVariable(variableName) != null;

    public IDictionary<string, string> List()
    {
        var dict = new Dictionary<string, string>();
        foreach (DictionaryEntry entry in System.Environment.GetEnvironmentVariables())
        {
            if (entry.Key is not string name)
                continue;

            if (entry.Value is string v)
                dict[name] = v;
        }

        return dict;
    }

    public void Remove(string variableName)
        => System.Environment.SetEnvironmentVariable(variableName, null);

    public string Directory(KnownDirectory directory)
        => Env.Directory(directory);

    public string Directory(KnownDirectory directory, KnownDirectoryOption option)
        => Env.Directory(directory, option);

    public string Expand(string template, EnvExpandOptions? options = null)
        => Env.Expand(template, options);

    public ReadOnlySpan<char> Expand(ReadOnlySpan<char> template, EnvExpandOptions? options = null)
        => Env.Expand(template, options);

    public void Set(string variableName, string value)
        => System.Environment.SetEnvironmentVariable(variableName, value);

    public void Set(string variableName, string value, bool secret)
    {
        if (secret)
            this.SecretMasker.Add(value);

        System.Environment.SetEnvironmentVariable(variableName, value);
    }

    public bool TryGet(string variableName, out string? value)
    {
        value = System.Environment.GetEnvironmentVariable(variableName);
        return value != null;
    }
#endif

    private sealed class DefaultVirtualEnvironmentPath : IVirtualEnvironmentPath
    {
        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();

        public IEnumerator<string> GetEnumerator()
            => Env.SplitPath().GetEnumerator();

#if NETLEGACY

        public void Add(string path, bool prepend = false)
        {
            var list = new List<string>(this.Split());
            if (Env.IsWindows)
            {
                foreach (var p in list)
                {
                    if (p.EqualsIgnoreCase(path))
                        return;
                }
            }
            else
            {
                foreach (var p in list)
                {
                    if (p.Equals(path))
                        return;
                }
            }

            if (prepend)
                list.Insert(0, path);
            else
                list.Add(path);

            var pathVar = Env.IsWindows ? "Path" : "PATH";
            Env.Set(pathVar, string.Join(System.IO.Path.PathSeparator.ToString(), list));
        }

        public void Remove(string path)
        {
            var paths = Env.SplitPath();
            var group = new List<string>();
            bool changed = false;
            if (Env.IsWindows)
            {
                foreach (var p in paths)
                {
                    if (p.Equals(path, StringComparison.OrdinalIgnoreCase))
                    {
                        changed = true;
                        continue;
                    }

                    group.Add(p);
                }
            }
            else
            {
                foreach (var p in paths)
                {
                    if (p.Equals(path, StringComparison.Ordinal))
                    {
                        changed = true;
                        continue;
                    }

                    group.Add(p);
                }
            }

            if (!changed)
                return;

            var pathVar = Env.IsWindows ? "Path" : "PATH";
            Env.Set(pathVar, string.Join(System.IO.Path.PathSeparator.ToString(), group));
        }

        public bool Has(string path)
        {
            if (Env.IsWindows)
            {
                foreach (var p in Env.SplitPath())
                {
                    if (p.Equals(path, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }

                return false;
            }

            foreach (var p in Env.SplitPath())
            {
                if (p.Equals(path))
                    return true;
            }

            return false;
        }

        public void Set(string paths)
        {
            var pathVar = Env.IsWindows ? "Path" : "PATH";
            Env.Set(pathVar, paths);
        }

        public IEnumerable<string> Split()
            => Env.SplitPath();

        public string Get()
        {
            var pathVar = Env.IsWindows ? "Path" : "PATH";
            return Env.Get(pathVar) ?? string.Empty;
        }
#endif
    }
}