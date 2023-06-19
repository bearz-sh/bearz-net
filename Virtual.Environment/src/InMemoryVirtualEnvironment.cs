using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using Bearz.Secrets;

namespace Bearz.Virtual.Environment;

public partial class InMemoryVirtualEnvironment : IVirtualEnvironment
{
    private readonly ConcurrentDictionary<string, string> variables;

    private readonly ISecretMasker masker;

    private readonly InMemoryVirtualEnvironmentPath path;

    public InMemoryVirtualEnvironment(InMemoryVirtualEnvironmentOptions? options = null, ISecretMasker? masker = null)
    {
        this.masker = masker ?? Secrets.SecretMasker.Default;
        var o = options ?? new InMemoryVirtualEnvironmentOptions();
        this.Cwd = o.Cwd;
        if (o.Variables != null)
        {
            this.variables = new ConcurrentDictionary<string, string>(o.Variables);
            this.path = new InMemoryVirtualEnvironmentPath(this);
            return;
        }

        var env = System.Environment.GetEnvironmentVariables();
        var dict = new Dictionary<string, string>();
        foreach (var key in env.Keys)
        {
            if (key is not string name)
            {
                continue;
            }

            var value = env[name];
            if (value is string v)
                dict[name] = v;
        }

        this.variables = new ConcurrentDictionary<string, string>(dict);
        this.path = new InMemoryVirtualEnvironmentPath(this);
    }

    public string Cwd { get; set; }

    public bool IsWindows => Env.IsWindows;

    public bool IsLinux => Env.IsLinux;

    public bool IsMacOS => Env.IsMacOS;

    public bool Is64BitProcess => Env.Is64BitProcess;

    public bool Is64BitOs => Env.Is64BitOs;

    public bool IsUserInteractive { get; set; }

    public bool IsPrivilegedProcess => Env.IsPrivilegedProcess;

    public bool IsWsl => Env.IsWsl;

    public Architecture OsArch => Env.OsArch;

    public Architecture ProcessArch => Env.ProcessArch;

    public IVirtualEnvironmentPath Path => this.path;

    public ISecretMasker SecretMasker => this.masker;

    public string User => Env.User;

    public string UserDomain => Env.UserDomain;

    public string Host => Env.Host;

    public string Home => Env.Home;

    public string? this[string key]
    {
        get
        {
            if (this.variables.TryGetValue(key, out var value))
                return value;

            return null;
        }

        set
        {
            if (value is null)
            {
                this.variables.TryRemove(key, out _);
                return;
            }

            this.variables[key] = value;
        }
    }

    public string? Get(string variableName)
        => this.variables.TryGetValue(variableName, out var value) ? value : null;

    public bool Has(string variableName)
        => this.variables.ContainsKey(variableName);

    public IDictionary<string, string> List()
    {
        var dict = new Dictionary<string, string>(this.variables);
        return dict;
    }

    public void Remove(string variableName)
    {
        this.variables.TryRemove(variableName, out _);
    }

    public string Directory(KnownDirectory directory)
        => Env.Directory(directory);

    public string Directory(KnownDirectory directory, KnownDirectoryOption option)
        => Env.Directory(directory, option);

    public string Expand(string template, EnvExpandOptions? options = null)
    {
        options ??= new EnvExpandOptions();
        options.GetVariable ??= this.Get;
        options.SetVariable ??= this.Set;

        return Env.Expand(template, options);
    }

    public ReadOnlySpan<char> Expand(ReadOnlySpan<char> template, EnvExpandOptions? options = null)
    {
        options ??= new EnvExpandOptions();
        options.GetVariable ??= this.Get;
        options.SetVariable ??= this.Set;

        return Env.Expand(template, options);
    }

    public void Set(string variableName, string value)
        => this.variables[variableName] = value;

    public void Set(string variableName, string value, bool secret)
    {
        if (secret)
            this.SecretMasker.Add(value);

        this.variables[variableName] = value;
    }

    public bool TryGet(string variableName, [NotNullWhen(true)] out string? value)
        => this.variables.TryGetValue(variableName, out value);
}