namespace Bearz.Virtual.Path;

public class DefaultVirtualPath : IVirtualPath
{
    public char PathSeparator => System.IO.Path.PathSeparator;

    public char AltDirectorySeparator => System.IO.Path.AltDirectorySeparatorChar;

    public char DirectorySeparator => System.IO.Path.DirectorySeparatorChar;

    public char VolumeSeparator => System.IO.Path.VolumeSeparatorChar;

#if NETLEGACY

    public string? ChangeExtension(string? path, string? extension)
        => System.IO.Path.ChangeExtension(path, extension);

    public string Combine(string path1, string path2)
        => System.IO.Path.Combine(path1, path2);

    public string Combine(string path1, string path2, string path3)
        => System.IO.Path.Combine(path1, path2, path3);

    public string Combine(string path1, string path2, string path3, string path4)
        => System.IO.Path.Combine(path1, path2, path3, path4);

    public string Combine(params string[] paths)
        => System.IO.Path.Combine(paths);

    public bool Exists(string? path)
        => System.IO.File.Exists(path) || System.IO.Directory.Exists(path);

    public string? GetDirectoryName(string? path)
        => System.IO.Path.GetDirectoryName(path);

    public string? GetExtension(string? path)
        => System.IO.Path.GetExtension(path);

    public string? GetFileName(string path)
        => System.IO.Path.GetFileName(path);

    public string? GetFileNameWithoutExtension(string path)
        => System.IO.Path.GetFileNameWithoutExtension(path);

    public string GetFullPath(string path)
        => System.IO.Path.GetFullPath(path);

    public string GetPathRoot(string path)
        => System.IO.Path.GetPathRoot(path);

    public string GetRandomFileName()
        => System.IO.Path.GetRandomFileName();

    public string GetTempPath()
        => System.IO.Path.GetTempPath();

    public bool HasExtension(string path)
        => System.IO.Path.HasExtension(path);

    public bool IsPathRooted(string? path)
        => System.IO.Path.IsPathRooted(path);
#endif
}