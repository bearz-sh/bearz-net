#if !NETLEGACY
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Bearz;

[StructLayout(LayoutKind.Sequential)]
public struct PathStr : IEnumerable<char>
{
    private static readonly ConditionalWeakTable<char[], string> cache = new ConditionalWeakTable<char[], string>();

    private readonly char[] path;
    private readonly int length;

    public PathStr(ReadOnlySpan<char> path)
    {
        this.path = path.ToArray();
    }

    public PathStr(StringBuilder path)
    {
        var span = new char[path.Length];
        path.CopyTo(0, span, 0, path.Length);

        this.path = span;
    }

    public int Length => this.path.Length;

    public bool IsEmpty => this.path.Length == 0;

    public char this[int index] => this.path[index];

    public static implicit operator PathStr(StringBuilder path)
    {
        return new PathStr(path);
    }

    public static implicit operator PathStr(ReadOnlySpan<char> path)
    {
        return new PathStr(path);
    }

    public static implicit operator string(PathStr path)
    {
        return path.ToString();
    }

    public static implicit operator ReadOnlySpan<char>(PathStr path)
    {
        return path.path;
    }

    public static PathStr From(Uri uri)
    {
        return new PathStr(uri.LocalPath);
    }

    public static PathStr From(string path)
    {
        var span = new char[path.Length];
        path.CopyTo(0, span, 0, path.Length);
        cache.Add(span, path);

        return new PathStr(path);
    }

    public static PathStr From(ReadOnlySpan<char> path)
    {
        return new PathStr(path);
    }

    public static PathStr From(StringBuilder path)
    {
        return new PathStr(path);
    }

    public IEnumerator<char> GetEnumerator()
    {
        foreach (var c in this.path)
            yield return c;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    public bool Contains(ReadOnlySpan<char> value, StringComparison comparisonType = StringComparison.Ordinal)
        => this.ToReadOnlySpan().Contains(value, comparisonType);

    public PathStr DirectoryName()
        => System.IO.Path.GetDirectoryName(this.path);

    public bool EndsWith(ReadOnlySpan<char> value, StringComparison comparisonType = StringComparison.Ordinal)
        => this.ToReadOnlySpan().EndsWith(value, comparisonType);

    public PathStr Extension()
        => System.IO.Path.GetExtension(this.path);

    // TODO: see if we can pass this.path directly stat and the windows native functions
    public bool Exists()
    {
        if (this.Length == 0)
            return false;

#if NET7_0_OR_GREATER
        return System.IO.Path.Exists(this.ToString());
#else
        return System.IO.File.Exists(this.ToString()) ||
               System.IO.Directory.Exists(this.ToString());
#endif
    }

    public IEnumerable<PathStr> EnumerateFiles()
    {
        if (this.Length == 0)
            yield break;

        foreach (var path in System.IO.Directory.EnumerateFiles(this.ToString()))
            yield return From(path);
    }

    public IEnumerable<PathStr> EnumerateFiles(string searchPattern)
    {
        if (this.Length == 0)
            yield break;

        foreach (var path in System.IO.Directory.EnumerateFiles(this.ToString(), searchPattern.ToString()))
            yield return From(path);
    }

    public IEnumerable<PathStr> EnumerateFiles(string searchPattern, System.IO.SearchOption searchOption)
    {
        if (this.Length == 0)
            yield break;

        foreach (var path in System.IO.Directory.EnumerateFiles(this.ToString(), searchPattern.ToString(), searchOption))
            yield return From(path);
    }

    public IEnumerable<PathStr> EnumerateDirectories()
    {
        if (this.Length == 0)
            yield break;

        foreach (var path in System.IO.Directory.EnumerateDirectories(this.ToString()))
            yield return From(path);
    }

    public IEnumerable<PathStr> EnumerateDirectories(string searchPattern)
    {
        if (this.Length == 0)
            yield break;

        foreach (var path in System.IO.Directory.EnumerateDirectories(this.ToString(), searchPattern.ToString()))
            yield return From(path);
    }

    public IEnumerable<PathStr> EnumerateDirectories(string searchPattern, System.IO.SearchOption searchOption)
    {
        if (this.Length == 0)
            yield break;

        foreach (var path in System.IO.Directory.EnumerateDirectories(this.ToString(), searchPattern.ToString(), searchOption))
            yield return From(path);
    }

    public IEnumerable<PathStr> EnumerateFileSystemEntries()
    {
        if (this.Length == 0)
            yield break;

        foreach (var path in System.IO.Directory.EnumerateFileSystemEntries(this.ToString()))
            yield return From(path);
    }

    public IEnumerable<PathStr> EnumerateFileSystemEntries(string searchPattern)
    {
        if (this.Length == 0)
            yield break;

        foreach (var path in System.IO.Directory.EnumerateFileSystemEntries(this.ToString(), searchPattern.ToString()))
            yield return From(path);
    }

    public IEnumerable<PathStr> EnumerateFileSystemEntries(string searchPattern, System.IO.SearchOption searchOption)
    {
        if (this.Length == 0)
            yield break;

        foreach (var path in System.IO.Directory.EnumerateFileSystemEntries(this.ToString(), searchPattern.ToString(), searchOption))
            yield return From(path);
    }

    public PathStr FileName()
        => System.IO.Path.GetFileName(this.path);

    public PathStr FileNameWithoutExtension()
        => System.IO.Path.GetFileNameWithoutExtension(this.path);

    public bool HasRoot()
        => System.IO.Path.IsPathRooted(this.path);

    public bool IsDirectory()
         => System.IO.File.GetAttributes(this.ToString()).HasFlag(System.IO.FileAttributes.Directory);

    public bool IsFile()
        => !this.IsDirectory();

    public bool IsSymlink()
    {
        if (this.Length == 0)
            return false;

        return System.IO.File.GetAttributes(this.ToString()).HasFlag(System.IO.FileAttributes.ReparsePoint);
    }

    public PathBuf Join(ReadOnlySpan<char> path)
    {
        return PathBuf.New(this).Join(path);
    }

    public PathStr Root()
        => System.IO.Path.GetPathRoot(this.path);

    public bool StartsWith(ReadOnlySpan<char> value, StringComparison comparisonType = StringComparison.Ordinal)
        => this.ToReadOnlySpan().StartsWith(value, comparisonType);

    public PathStr SetExtension(ReadOnlySpan<char> extension)
    {
        var path = this.ToString();
        var ext = extension.ToArray();
        if (!cache.TryGetValue(ext, out var extString))
        {
            extString = extension.ToString();
            cache.Add(ext, extString);
        }

        var newPath = Path.ChangeExtension(path, extString);
        return new PathStr(newPath);
    }

    public char[] ToArray()
    {
        return this.path;
    }

    public ReadOnlySpan<char> ToReadOnlySpan()
    {
        return this.path;
    }

    public override string ToString()
    {
        if (cache.TryGetValue(this.path, out var value))
            return value;

        var str = new string(this.path);
        cache.Add(this.path, str);
        return str;
    }
}
#endif