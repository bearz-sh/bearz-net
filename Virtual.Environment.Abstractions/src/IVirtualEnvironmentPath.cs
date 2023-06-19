using Bearz.Extra.Strings;

namespace Bearz.Virtual;

#if NET5_0_OR_GREATER

public interface IVirtualEnvironmentPath : IEnumerable<string>
{
    void Add(string path, bool prepend = false)
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
        Env.Set(pathVar, string.Join(Path.PathSeparator.ToString(), list));
    }

    void Remove(string path)
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
        Env.Set(pathVar, string.Join(Path.PathSeparator, group));
    }

    IEnumerable<string> Split()
        => Env.SplitPath();

    bool Has(string path)
    {
        var paths = Env.SplitPath();
        if (Env.IsWindows)
            return paths.Contains(path, StringComparer.OrdinalIgnoreCase);

        return paths.Contains(path);
    }

    void Set(string paths)
    {
        var pathVar = Env.IsWindows ? "Path" : "PATH";
        Env.Set(pathVar, paths);
    }

    string Get()
    {
        var pathVar = Env.IsWindows ? "Path" : "PATH";
        return Env.Get(pathVar) ?? string.Empty;
    }
}

#else

public interface IVirtualEnvironmentPath : IEnumerable<string>
{
    void Add(string path, bool prepend = false);

    void Remove(string path);

    bool Has(string path);

    void Set(string paths);

    IEnumerable<string> Split();

    string Get();
}

#endif