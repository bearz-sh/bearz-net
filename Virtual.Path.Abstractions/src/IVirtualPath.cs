#if !NET5_0_OR_GREATER

using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Bearz.Virtual;

public partial interface IVirtualPath
{
    char PathSeparator { get; }

    char AltDirectorySeparator { get; }

    char DirectorySeparator { get; }

    char VolumeSeparator { get; }

    string? ChangeExtension(string? path, string? extension);

    string Combine(string path1, string path2);

    string Combine(string path1, string path2, string path3);

    string Combine(string path1, string path2, string path3, string path4);

    string Combine(params string[] paths);

    bool Exists([NotNullWhen(true)] string? path);

    string? GetDirectoryName(string? path);

    [Pure]
    [return: NotNullIfNotNull("path")]
    string? GetExtension(string? path);

    [return: NotNullIfNotNull("path")]
    string? GetFileName(string path);

    [return: NotNullIfNotNull("path")]
    string? GetFileNameWithoutExtension(string path);

    string GetFullPath(string path);

    string GetPathRoot(string path);

    string GetRandomFileName();

    string GetTempPath();

    bool HasExtension(string path);

    bool IsPathRooted([NotNullWhen(true)] string? path);
}

#endif