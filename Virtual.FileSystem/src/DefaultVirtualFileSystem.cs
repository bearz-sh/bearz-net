using System.Text;

namespace Bearz.Virtual.FileSystem;

public class DefaultVirtualFileSystem : IVirtualFileSystem
{
#if NETLEGACY
    public void CopyDirectory(string sourceDir, string destinationDir, bool recursive, bool force = false)
        => Fs.CopyDirectory(sourceDir, destinationDir, recursive, force);

    public void CopyFile(string source, string destination)
        => File.Copy(source, destination);

    public void CopyFile(string source, string destination, bool overwrite)
        => File.Copy(source, destination, overwrite);

    public IEnumerable<string> EnumerateTextFile(string path)
        => File.ReadLines(path);

    public IEnumerable<string> EnumerateFiles(string path)
        => Directory.EnumerateFiles(path);

    public IEnumerable<string> EnumerateFiles(string path, string searchPattern)
        => Directory.EnumerateFiles(path, searchPattern);

    public IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        => Directory.EnumerateFiles(path, searchPattern, searchOption);

    public IEnumerable<string> EnumerateDirectories(string path)
        => Directory.EnumerateDirectories(path);

    public IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
        => Directory.EnumerateDirectories(path, searchPattern);

    public IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        => Directory.EnumerateDirectories(path, searchPattern, searchOption);

    public IEnumerable<string> EnumerateFileSystemEntries(string path)
        => Directory.EnumerateFileSystemEntries(path);

    public IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern)
        => Directory.EnumerateFileSystemEntries(path, searchPattern);

    public IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
        => Directory.EnumerateFileSystemEntries(path, searchPattern, searchOption);

    public void EnsureDirectory(string path)
    {
        if (!this.DirectoryExists(path))
            Directory.CreateDirectory(path);
    }

    public void EnsureFile(string path)
    {
        if (!this.FileExists(path))
            File.WriteAllBytes(path, Array.Empty<byte>());
    }

    public bool DirectoryExists(string path)
        => Directory.Exists(path);

    public bool FileExists(string path)
        => File.Exists(path);

    public bool IsDirectory(string path)
    {
        try
        {
            return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }
        catch
        {
            return false;
        }
    }

    public bool IsFile(string path)
    {
        try
        {
            return !File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }
        catch
        {
            return false;
        }
    }

    public void MakeDirectory(string path)
        => Directory.CreateDirectory(path);

    public void MoveDirectory(string sourceDir, string destinationDir)
        => Directory.Move(sourceDir, destinationDir);

    public void MoveFile(string source, string destination)
        => File.Move(source, destination);

    public FileStream OpenCreateFileStream(string path)
        => File.Create(path);

    public FileStream OpenCreateFileStream(string path, int bufferSize)
        => File.Create(path, bufferSize);

    public FileStream OpenCreateFileStream(string path, int bufferSize, FileOptions options)
        => File.Create(path, bufferSize, options);

    public FileStream OpenFileStream(string path, FileMode mode)
        => File.Open(path, mode);

    public FileStream OpenFileStream(string path, FileMode mode, FileAccess access)
        => File.Open(path, mode, access);

    public FileStream OpenFileStream(string path, FileMode mode, FileAccess access, FileShare share)
        => File.Open(path, mode, access, share);

    public FileStream OpenReadFileStream(string path)
        => File.OpenRead(path);

    public FileStream OpenWriteFileSteam(string path)
        => File.OpenWrite(path);

    public StreamWriter OpenWriter(string path, bool append = false)
        => append ? File.AppendText(path) : File.CreateText(path);

    public void RemoveDirectory(string path)
        => Directory.Delete(path);

    public void RemoveFile(string path)
        => File.Delete(path);

    public byte[] ReadFile(string path)
        => File.ReadAllBytes(path);

    public string ReadTextFile(string path, Encoding? encoding = null)
        => encoding is not null ? File.ReadAllText(path, encoding) : File.ReadAllText(path);

    public void WriteFile(string path, byte[] contexts)
        => File.WriteAllBytes(path, contexts);

    public void WriteTextFile(string path, string contents, Encoding? encoding = null, bool append = false)
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

    public void WriteTextFile(string path, IEnumerable<string> contents, Encoding? encoding = null, bool append = false)
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
#endif
}