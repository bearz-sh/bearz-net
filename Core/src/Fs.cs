using System.Collections;
using System.Text;

using Microsoft.Win32.SafeHandles;

namespace Bearz;

public static class Fs
{
    public static void CopyDirectory(PathStr sourceDir, PathStr destinationDir, bool recursive, bool force = false)
    {
        if (!sourceDir.Exists())
            throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");

        if (!DirectoryExists(destinationDir))
            Directory.CreateDirectory(destinationDir);

        foreach (var file in EnumerateFiles(sourceDir))
        {
            var buf = new PathBuf(destinationDir);
            var target = buf.Join(file.FileName()).ToPathStr();
            if (target.Exists())
            {
                if (force)
                    Fs.RemoveDirectory(target);
                else
                    throw new IOException($"File already exists: {target}");
            }

            CopyFile(file, target);
        }

        if (!recursive)
        {
            return;
        }

        foreach (var dir in sourceDir.EnumerateDirectories())
        {
            var dest = PathBuf.New(dir).Join(dir.FileName()).ToPathStr();
            CopyDirectory(dir, dest, recursive, force);
        }
    }

    public static void CopyFile(PathStr source, PathStr destination)
    {
        File.Copy(source, destination);
    }

    public static void CopyFile(PathStr source, PathStr destination, bool overwrite)
    {
        File.Copy(source, destination, overwrite);
    }

    public static void Chmod(PathStr path, UnixFileMode mode)
    {
        File.SetUnixFileMode(path, mode);
    }

    public static FileStream CreateFile(PathStr path)
        => File.Create(path);

    public static FileStream CreateFile(PathStr path, int bufferSize)
        => File.Create(path, bufferSize);

    public static FileStream CreateFile(PathStr path, int bufferSize, FileOptions options)
        => File.Create(path, bufferSize, options);

    public static StreamWriter CreateTextFile(PathStr path, bool append = false)
    {
        if (append)
            return File.AppendText(path);

        return File.CreateText(path);
    }

    public static IEnumerable<string> EnumerateTextFile(PathStr path)
    {
        return File.ReadLines(path);
    }

    public static IAsyncEnumerable<string> EnumerateTextFileAsync(PathStr path, CancellationToken cancellationToken = default)
    {
        return File.ReadLinesAsync(path, cancellationToken);
    }

    public static IEnumerable<PathStr> EnumerateFiles(PathStr path)
    {
        foreach (var item in Directory.EnumerateFiles(path))
        {
            yield return PathStr.From(item);
        }
    }

    public static IEnumerable<PathStr> EnumerateFiles(PathStr path, string searchPattern)
    {
        foreach (var item in Directory.EnumerateFiles(path, searchPattern))
        {
            yield return PathStr.From(item);
        }
    }

    public static IEnumerable<PathStr> EnumerateFiles(PathStr path, string searchPattern, SearchOption searchOption)
    {
        foreach (var item in Directory.EnumerateFiles(path, searchPattern, searchOption))
        {
            yield return PathStr.From(item);
        }
    }

    public static IEnumerable<PathStr> EnumerateDirectories(PathStr path)
    {
        foreach (var item in Directory.EnumerateDirectories(path))
        {
            yield return PathStr.From(item);
        }
    }

    public static IEnumerable<PathStr> EnumerateDirectories(PathStr path, string searchPattern)
    {
        foreach (var item in Directory.EnumerateDirectories(path, searchPattern))
        {
            yield return PathStr.From(item);
        }
    }

    public static IEnumerable<PathStr> EnumerateDirectories(PathStr path, string searchPattern, SearchOption searchOption)
    {
        foreach (var item in Directory.EnumerateDirectories(path, searchPattern, searchOption))
        {
            yield return PathStr.From(item);
        }
    }

    public static IEnumerable<PathStr> EnumerateFileSystemEntries(PathStr path)
    {
        foreach (var item in Directory.EnumerateFileSystemEntries(path))
        {
            yield return PathStr.From(item);
        }
    }

    public static void EnsureDirectory(PathStr path)
    {
        if (!DirectoryExists(path))
            Directory.CreateDirectory(path);
    }

    public static void EnsureDirectory(PathStr path, UnixFileMode mode)
    {
        if (!DirectoryExists(path))
            Directory.CreateDirectory(path, mode);
    }

    public static void EnsureFile(PathStr path)
    {
        if (!FileExists(path))
            File.WriteAllBytes(path, Array.Empty<byte>());
    }

    public static bool DirectoryExists(PathStr path)
        => Directory.Exists(path);

    public static bool FileExists(PathStr path)
        => File.Exists(path);

    public static bool IsDirectory(PathStr path)
        => File.GetAttributes(path).HasFlag(FileAttributes.Directory);

    public static bool IsFile(PathStr path)
        => !IsDirectory(path);

    public static void LinkDirectory(PathStr path, PathStr pathToTarget)
        => Directory.CreateSymbolicLink(path, pathToTarget);

    public static void LinkFile(PathStr path, PathStr pathToTarget)
        => File.CreateSymbolicLink(path, pathToTarget);

    public static void MakeDirectory(PathStr path)
        => Directory.CreateDirectory(path);

    public static void MakeDirectory(PathStr path, UnixFileMode mode)
        => Directory.CreateDirectory(path, mode);

    public static void MoveDirectory(PathStr source, PathStr destination)
        => Directory.Move(source, destination);

    public static void MoveFile(PathStr source, PathStr destination)
        => File.Move(source, destination);

    public static FileStream OpenFile(PathStr path)
        => File.OpenRead(path);

    public static FileStream OpenFile(PathStr path, FileMode mode)
        => File.Open(path, mode);

    public static FileStream OpenFile(PathStr path, FileMode mode, FileAccess access)
        => File.Open(path, mode, access);

    public static FileStream OpenFile(PathStr path, FileMode mode, FileAccess access, FileShare share)
        => File.Open(path, mode, access, share);

    public static FileStream OpenFileWriteStream(PathStr path)
        => File.OpenWrite(path);

    public static SafeFileHandle OpenFileHandle(PathStr path)
        => File.OpenHandle(path);

    public static void RemoveFile(PathStr path)
        => File.Delete(path);

    public static void RemoveDirectory(PathStr path, bool recursive = false)
        => Directory.Delete(path, recursive);

    public static byte[] ReadFile(PathStr path)
    {
        return File.ReadAllBytes(path);
    }

    public static string ReadTextFile(PathStr path)
    {
        return File.ReadAllText(path);
    }

    public static void WriteFile(PathStr path, byte[] data)
    {
        File.WriteAllBytes(path, data);
    }

    public static Task WriteFileAsync(PathStr path, byte[] data, CancellationToken cancellationToken = default)
    {
        return File.WriteAllBytesAsync(path, data, cancellationToken);
    }

    public static void WriteTextFile(PathStr path, string contents, Encoding? encoding = null, bool append = false)
    {
        if (append)
        {
            if (encoding is not null)
            {
                File.AppendAllText(path, contents, encoding);
                return;
            }

            File.AppendAllText(path, contents);
            return;
        }

        if (encoding is not null)
        {
            File.WriteAllText(path, contents, encoding);
            return;
        }

        File.WriteAllText(path, contents);
    }

    public static void WriteTextFile(
        PathStr path,
        IEnumerable<string> contents,
        Encoding? encoding = null,
        bool append = false)
    {
        if (append)
        {
            if (encoding is not null)
            {
                File.AppendAllLines(path, contents, encoding);
                return;
            }

            File.AppendAllLines(path, contents);
            return;
        }

        if (encoding is not null)
        {
            File.WriteAllLines(path, contents, encoding);
            return;
        }

        File.WriteAllLines(path, contents);
    }

    public static Task WriteTextFileAsync(PathStr path, string contents, Encoding? encoding = null, bool append = false, CancellationToken cancellationToken = default)
    {
        if (append)
        {
            if (encoding is not null)
            {
                return File.AppendAllTextAsync(path, contents, encoding, cancellationToken);
            }

            return File.AppendAllTextAsync(path, contents, cancellationToken);
        }

        if (encoding is not null)
        {
            return File.WriteAllTextAsync(path, contents, encoding, cancellationToken);
        }

        return File.WriteAllTextAsync(path, contents, cancellationToken);
    }

    public static Task WriteTextFileAsync(PathStr path, IEnumerable<string> contents, Encoding? encoding = null, bool append = false, CancellationToken cancellationToken = default)
    {
        if (append)
        {
            if (encoding is not null)
            {
                return File.AppendAllLinesAsync(path, contents, encoding, cancellationToken);
            }

            return File.AppendAllLinesAsync(path, contents, cancellationToken);
        }

        if (encoding is not null)
        {
            return File.WriteAllLinesAsync(path, contents, encoding, cancellationToken);
        }

        return File.WriteAllLinesAsync(path, contents, cancellationToken);
    }
}