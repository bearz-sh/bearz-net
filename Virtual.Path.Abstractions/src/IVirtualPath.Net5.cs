#if NET5_0_OR_GREATER

using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Bearz.Virtual;

public partial interface IVirtualPath
{
    char PathSeparator { get; }

    char AltDirectorySeparator { get; }

    char DirectorySeparator { get; }

    char VolumeSeparator { get; }

    string? ChangeExtension(string? path, string? extension)
        => Path.ChangeExtension(path, extension);

    string Combine(string path1, string path2)
        => Path.Combine(path1, path2);

    string Combine(string path1, string path2, string path3)
        => Path.Combine(path1, path2, path3);

    string Combine(string path1, string path2, string path3, string path4)
        => Path.Combine(path1, path2, path3, path4);

    string Combine(params string[] paths)
        => Path.Combine(paths);

    bool EndsWithDirectorySeparator(string path)
        => Path.EndsInDirectorySeparator(path);

    bool EndsWithDirectorySeparator(ReadOnlySpan<char> path)
        => Path.EndsInDirectorySeparator(path);

#if NET7_0_OR_GREATER
    bool Exists([NotNullWhen(true)] string? path)
        => Path.Exists(path);
#else
    bool Exists([NotNullWhen(true)] string? path)
        => File.Exists(path) || Directory.Exists(path);
#endif

    string? GetDirectoryName(string? path)
        => Path.GetDirectoryName(path);

    ReadOnlySpan<char> GetDirectoryName(ReadOnlySpan<char> path)
        => Path.GetDirectoryName(path);

    [Pure]
    [return: NotNullIfNotNull("path")]
    string? GetExtension(string? path)
        => Path.GetExtension(path);

    ReadOnlySpan<char> GetExtension(ReadOnlySpan<char> path)
        => Path.GetExtension(path);

    [return: NotNullIfNotNull("path")]
    string? GetFileName(string path)
        => Path.GetFileName(path);

    ReadOnlySpan<char> GetFileName(ReadOnlySpan<char> path)
        => Path.GetFileName(path);

    [return: NotNullIfNotNull("path")]
    string? GetFileNameWithoutExtension(string path)
        => Path.GetFileNameWithoutExtension(path);

    ReadOnlySpan<char> GetFileNameWithoutExtension(ReadOnlySpan<char> path)
        => Path.GetFileNameWithoutExtension(path);

    string GetFullPath(string path)
        => Path.GetFullPath(path);

    string GetFullPath(string path, string basePath)
        => Path.GetFullPath(path, basePath);

    string? GetPathRoot(string? path)
        => Path.GetPathRoot(path);

    ReadOnlySpan<char> GetPathRoot(ReadOnlySpan<char> path)
        => Path.GetPathRoot(path);

    string GetRandomFileName()
        => Path.GetRandomFileName();

    string GetTempPath()
        => Path.GetTempPath();

    bool HasExtension(string path)
        => Path.HasExtension(path);

    bool HasExtension(ReadOnlySpan<char> path)
        => Path.HasExtension(path);

    bool IsPathRooted([NotNullWhen(true)] string? path)
        => Path.IsPathRooted(path);

    bool IsPathRooted(ReadOnlySpan<char> path)
        => Path.IsPathRooted(path);

    bool IsPathFullyQualified(string path)
        => Path.IsPathFullyQualified(path);

    bool IsPathFullyQualified(ReadOnlySpan<char> path)
        => Path.IsPathFullyQualified(path);

    string Join(string path1, string path2)
        => Path.Join(path1, path2);

    string Join(string path1, string path2, string path3)
        => Path.Join(path1, path2, path3);

    string Join(params string[] paths)
        => Path.Join(paths);

    string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2)
        => Path.Join(path1, path2);

    string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, ReadOnlySpan<char> path3)
        => Path.Join(path1, path2, path3);

    string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, ReadOnlySpan<char> path3, ReadOnlySpan<char> path4)
        => Path.Join(path1, path2, path3, path4);

    string TrimEndingDirectorySeparator(string path)
        => Path.TrimEndingDirectorySeparator(path);

    ReadOnlySpan<char> TrimEndingDirectorySeparator(ReadOnlySpan<char> path)
        => Path.TrimEndingDirectorySeparator(path);

    bool TryJoin(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, Span<char> destination, out int charsWritten)
        => Path.TryJoin(path1, path2, destination, out charsWritten);

    bool TryJoin(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, ReadOnlySpan<char> path3, Span<char> destination, out int charsWritten)
        => Path.TryJoin(path1, path2, path3, destination, out charsWritten);
}

#endif