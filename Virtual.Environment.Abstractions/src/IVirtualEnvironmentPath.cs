namespace Bearz.Virtual;

#if NET5_0_OR_GREATER

public interface IVirtualEnvironmentPath : IEnumerable<string>
{
    void Add(string path, bool prepend = false)
    {
        if (this.Has(path))
            return;

        var pathVar = Env.IsWindows ? "Path" : "PATH";
        var paths = Env.Get(pathVar) ?? string.Empty;
        paths = prepend ?
            string.Join(Path.PathSeparator, path, paths) :
            string.Join(Path.PathSeparator, paths, path);

        Env.Set(pathVar, paths);
    }

    void Remove(string path)
    {
        var paths = Env.SplitPath();
        var group = new List<string>();
        if (Env.IsWindows)
        {
            foreach (var p in paths)
            {
                if (p.Equals(path, StringComparison.OrdinalIgnoreCase))
                {
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
                    continue;
                }

                group.Add(p);
            }
        }

        var pathVar = Env.IsWindows ? "Path" : "PATH";
        Env.Set(pathVar, string.Join(Path.PathSeparator, group));
    }

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

    string Get();
}

#endif