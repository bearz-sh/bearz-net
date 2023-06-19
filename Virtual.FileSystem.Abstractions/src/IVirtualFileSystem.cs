using System.Diagnostics.CodeAnalysis;
using System.Text;

using Microsoft.VisualBasic;
using Microsoft.Win32.SafeHandles;

namespace Bearz.Virtual;

public partial interface IVirtualFileSystem
{
#if NETLEGACY
    void CopyDirectory(string sourceDir, string destinationDir, bool recursive, bool force = false);

    void CopyFile(string source, string destination);

    void CopyFile(string source, string destination, bool overwrite);

    IEnumerable<string> EnumerateTextFile(string path);

    IEnumerable<string> EnumerateFiles(string path);

    IEnumerable<string> EnumerateFiles(string path, string searchPattern);

    IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption);

    IEnumerable<string> EnumerateDirectories(string path);

    IEnumerable<string> EnumerateDirectories(string path, string searchPattern);

    IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption);

    IEnumerable<string> EnumerateFileSystemEntries(string path);

    IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern);

    IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption);

    void EnsureDirectory(string path);

    void EnsureFile(string path);

    bool DirectoryExists(string path);

    bool FileExists(string path);

    bool IsDirectory(string path);

    bool IsFile(string path);

    void MakeDirectory(string path);

    void MoveDirectory(string sourceDir, string destinationDir);

    void MoveFile(string source, string destination);

    FileStream OpenCreateFileStream(string path);

    FileStream OpenCreateFileStream(string path, int bufferSize);

    FileStream OpenCreateFileStream(string path, int bufferSize, FileOptions options);

    FileStream OpenFileStream(string path, FileMode mode);

    FileStream OpenFileStream(string path, FileMode mode, FileAccess access);

    FileStream OpenFileStream(string path, FileMode mode, FileAccess access, FileShare share);

    FileStream OpenReadFileStream(string path);

    FileStream OpenWriteFileSteam(string path);

    StreamWriter OpenWriter(string path, bool append = false);

    void RemoveDirectory(string path);

    void RemoveFile(string path);

    byte[] ReadFile(string path);

    string ReadTextFile(string path, Encoding? encoding = null);

    void WriteFile(string path, byte[] contexts);

    void WriteTextFile(string path, string contents, Encoding? encoding = null, bool append = false);

    void WriteTextFile(string path, IEnumerable<string> contents, Encoding? encoding = null, bool append = false);
#else
    void CopyDirectory(string sourceDir, string destinationDir, bool recursive, bool force = false)
        => Fs.CopyDirectory(sourceDir, destinationDir, recursive, force);

    void CopyFile(string source, string destination)
        => File.Copy(source, destination);

    void CopyFile(string source, string destination, bool overwrite)
        => File.Copy(source, destination, overwrite);

    bool DirectoryExists(string path)
        => Directory.Exists(path);

    IEnumerable<string> EnumerateTextFile(string path)
        => File.ReadLines(path);

    IEnumerable<string> EnumerateFiles(string path)
        => Directory.EnumerateFiles(path);

    IEnumerable<string> EnumerateFiles(string path, string searchPattern)
        => Directory.EnumerateFiles(path, searchPattern);

    IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        => Directory.EnumerateFiles(path, searchPattern, searchOption);

    IEnumerable<string> EnumerateDirectories(string path)
        => Directory.EnumerateDirectories(path);

    IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
        => Directory.EnumerateDirectories(path, searchPattern);

    IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        => Directory.EnumerateDirectories(path, searchPattern, searchOption);

    IEnumerable<string> EnumerateFileSystemEntries(string path)
        => Directory.EnumerateFileSystemEntries(path);

    IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern)
        => Directory.EnumerateFileSystemEntries(path, searchPattern);

    IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
        => Directory.EnumerateFileSystemEntries(path, searchPattern, searchOption);

    [SuppressMessage("Minor Code Smell", "S4136:Method overloads should be grouped together")]
    void EnsureDirectory(string path)
    {
        if (!this.DirectoryExists(path))
            Directory.CreateDirectory(path);
    }

    void EnsureFile(string path)
    {
        if (!this.FileExists(path))
            File.WriteAllBytes(path, Array.Empty<byte>());
    }

    bool FileExists(string path)
        => File.Exists(path);

    bool IsDirectory(string path)
    {
        try
        {
            return File.GetAttributes(path).HasFlag(FileAttribute.Directory);
        }
        catch
        {
            return false;
        }
    }

    bool IsFile(string path)
        => File.Exists(path);

    void LinkDirectory(string path, string pathToTarget)
        => Directory.CreateSymbolicLink(path, pathToTarget);

    void LinkFile(string path, string pathToTarget)
        => File.CreateSymbolicLink(path, pathToTarget);

    [SuppressMessage("Minor Code Smell", "S4136:Method overloads should be grouped together")]
    void MakeDirectory(string path)
        => Directory.CreateDirectory(path);

    void MoveDirectory(string sourceDir, string destinationDir)
        => Directory.Move(sourceDir, destinationDir);

    void MoveFile(string source, string destination)
        => File.Move(source, destination);

    FileStream OpenCreateFileStream(string path)
        => File.Create(path);

    FileStream OpenCreateFileStream(string path, int bufferSize)
        => File.Create(path, bufferSize);

    FileStream OpenCreateFileStream(string path, int bufferSize, FileOptions options)
        => File.Create(path, bufferSize, options);

    FileStream OpenFileStream(string path, FileStreamOptions options)
        => File.Open(path, options);

    FileStream OpenFileStream(string path, FileMode mode)
        => File.Open(path, mode);

    FileStream OpenFileStream(string path, FileMode mode, FileAccess access)
        => File.Open(path, mode, access);

    FileStream OpenFileStream(string path, FileMode mode, FileAccess access, FileShare share)
        => File.Open(path, mode, access, share);

    FileStream OpenReadFileStream(string path)
        => File.OpenRead(path);

    FileStream OpenWriteFileSteam(string path)
        => File.OpenWrite(path);

    StreamWriter OpenWriter(string path, bool append = false)
        => append ? File.AppendText(path) : File.CreateText(path);

    void RemoveDirectory(string path)
        => Directory.Delete(path);

    void RemoveFile(string path)
        => File.Delete(path);

    byte[] ReadFile(string path)
        => File.ReadAllBytes(path);

    Task<byte[]> ReadFileAsync(string path, CancellationToken cancellationToken = default)
        => File.ReadAllBytesAsync(path, cancellationToken);

    string ReadTextFile(string path, Encoding? encoding = null)
        => encoding is not null ? File.ReadAllText(path, encoding) : File.ReadAllText(path);

    Task<string> ReadTextFileAsync(string path, Encoding? encoding = null, CancellationToken cancellationToken = default)
        => encoding is not null ? File.ReadAllTextAsync(path, encoding, cancellationToken) : File.ReadAllTextAsync(path, cancellationToken);

    FileSystemInfo? ResolveLinkedFile(string linkPath, bool returnFinalTarget = false)
        => File.ResolveLinkTarget(linkPath, returnFinalTarget);

    FileSystemInfo? ResolveLinkedDirectory(string linkPath, bool returnFinalTarget = false)
        => Directory.ResolveLinkTarget(linkPath, returnFinalTarget);

    void WriteFile(string path, byte[] contexts)
        => File.WriteAllBytes(path, contexts);

    void WriteTextFile(string path, string contents, Encoding? encoding = null, bool append = false)
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

    void WriteTextFile(string path, IEnumerable<string> contents, Encoding? encoding = null, bool append = false)
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

    Task WriteTextFileAsync(string path, string contents, Encoding? encoding = null, bool append = false, CancellationToken cancellationToken = default)
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

    Task WriteTextFileAsync(string path, IEnumerable<string> contents, Encoding? encoding = null, bool append = false, CancellationToken cancellationToken = default)
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

#endif

#if NET7_0_OR_GREATER
    IAsyncEnumerable<string> EnumerateTextFileAsync(string path, CancellationToken cancellationToken = default)
        => File.ReadLinesAsync(path, cancellationToken);

    UnixFileMode GetUnixFileMode(string path)
        => File.GetUnixFileMode(path);

    void MakeDirectory(string path, UnixFileMode mode)
        => Directory.CreateDirectory(path, mode);

    void EnsureDirectory(string path, UnixFileMode mode)
    {
        if (!this.DirectoryExists(path))
            Directory.CreateDirectory(path, mode);
    }

    void Chmod(string path, UnixFileMode mode)
        => File.SetUnixFileMode(path, mode);

    void Chmod(SafeFileHandle fileHandle, UnixFileMode mode)
        => File.SetUnixFileMode(fileHandle, mode);

    void SetUnixFileMode(string path, UnixFileMode mode)
        => File.SetUnixFileMode(path, mode);

    Task WriteFileAsync(string path, byte[] data, CancellationToken cancellationToken = default)
        => File.WriteAllBytesAsync(path, data, cancellationToken);
#endif
}