using System.Collections;

using Bearz.Extra.Strings;

namespace Bearz.Virtual.Environment;

public class InMemoryVirtualEnvironmentPath : IVirtualEnvironmentPath
{
    private readonly string pathKey;

    private readonly InMemoryVirtualEnvironment env;

    public InMemoryVirtualEnvironmentPath(InMemoryVirtualEnvironment env)
    {
        this.pathKey = Env.IsWindows ? "Path" : "PATH";
        this.env = env;
    }

    public IEnumerator<string> GetEnumerator()
        => this.Split().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => this.Split().GetEnumerator();

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

        this.env.Set(this.pathKey, string.Join(Path.PathSeparator.ToString(), list));
    }

    public string Get()
        => this.env.Get(this.pathKey) ?? string.Empty;

    public void Remove(string path)
    {
        var group = new List<string>();
        var changed = false;
        if (this.env.IsWindows)
        {
            foreach (var p in this.Split())
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
            foreach (var p in this.Split())
            {
                if (p.Equals(path))
                {
                    changed = true;
                    continue;
                }

                group.Add(p);
            }
        }

        if (!changed)
            return;

        this.env.Set(this.pathKey, string.Join(Path.PathSeparator.ToString(), group));
    }

    public bool Has(string path)
    {
        if (Env.IsWindows)
        {
            foreach (var p in this.Split())
            {
                if (p.EqualsIgnoreCase(path))
                    return true;
            }

            return false;
        }

        foreach (var p in this.Split())
        {
            if (p.Equals(path))
                return true;
        }

        return false;
    }

    public void Set(string paths)
        => this.env.Set(this.pathKey, paths);

    public IEnumerable<string> Split()
    {
        var paths = this.env.Get(this.pathKey) ?? string.Empty;
        foreach (var path in paths.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries))
        {
            yield return path;
        }
    }
}