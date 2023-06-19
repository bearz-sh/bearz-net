using System.Collections;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text;

using Bearz.Text;

using Microsoft.Win32.SafeHandles;

namespace Bearz;

public static partial class Fs
{
    public static string CatFiles(params string[] files)
    {
        return CatFiles((IEnumerable<string>)files);
    }

    public static string CatFiles(IEnumerable<string> files, bool throwIfNotFound = false)
    {
        var sb = StringBuilderCache.Acquire();
        foreach (var file in files)
        {
            if (throwIfNotFound && !File.Exists(file))
                throw new FileNotFoundException($"File not found: {file}");

            if (sb.Length > 0)
                sb.Append(Environment.NewLine);

            sb.Append(ReadTextFile(file));
        }

        return StringBuilderCache.GetStringAndRelease(sb);
    }

    public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive, bool force = false)
    {
        var dir = new DirectoryInfo(sourceDir);

        if (!dir.Exists)
            throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

        if (!DirectoryExists(destinationDir))
            Directory.CreateDirectory(destinationDir);

        foreach (FileInfo file in dir.GetFiles())
        {
            string targetFilePath = Path.Combine(destinationDir, file.Name);
            if (FileExists(targetFilePath))
            {
                if (force)
                    File.Delete(targetFilePath);
                else
                    throw new IOException($"File already exists: {targetFilePath}");
            }

            file.CopyTo(targetFilePath);
        }

        if (!recursive)
        {
            return;
        }

        DirectoryInfo[] dirs = dir.GetDirectories();
        foreach (DirectoryInfo subDir in dirs)
        {
            string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
            CopyDirectory(subDir.FullName, newDestinationDir, true, force);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CopyFile(string source, string destination)
        => File.Copy(source, destination);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CopyFile(string source, string destination, bool overwrite)
        => File.Copy(source, destination, overwrite);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FileStream CreateFile(string path)
        => File.Create(path);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FileStream CreateFile(string path, int bufferSize)
        => File.Create(path, bufferSize);

    public static FileStream CreateFile(string path, int bufferSize, FileOptions options)
        => File.Create(path, bufferSize, options);

    public static StreamWriter CreateTextFile(string path, bool append = false)
    {
        if (append)
            return File.AppendText(path);

        return File.CreateText(path);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<string> EnumerateTextFile(string path)
    {
        return File.ReadLines(path);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<string> EnumerateFiles(string path)
        => Directory.EnumerateFiles(path);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<string> EnumerateFiles(string path, string searchPattern)
        => Directory.EnumerateFiles(path, searchPattern);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        => Directory.EnumerateFiles(path, searchPattern, searchOption);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<string> EnumerateDirectories(string path)
        => Directory.EnumerateDirectories(path);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
        => Directory.EnumerateDirectories(path, searchPattern);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        => Directory.EnumerateDirectories(path, searchPattern, searchOption);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<string> EnumerateFileSystemEntries(string path)
        => Directory.EnumerateFileSystemEntries(path);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern)
        => Directory.EnumerateFileSystemEntries(path, searchPattern);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
        => Directory.EnumerateFileSystemEntries(path, searchPattern, searchOption);

    public static void EnsureDirectory(string path)
    {
        if (!DirectoryExists(path))
            Directory.CreateDirectory(path);
    }

    public static void EnsureFile(string path)
    {
        if (!FileExists(path))
            File.WriteAllBytes(path, Array.Empty<byte>());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool DirectoryExists(string path)
        => Directory.Exists(path);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool FileExists(string path)
        => File.Exists(path);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDirectory(string path)
        => File.GetAttributes(path).HasFlag(FileAttributes.Directory);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFile(string path)
        => !IsDirectory(path);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void MakeDirectory(string path)
        => Directory.CreateDirectory(path);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void MoveDirectory(string source, string destination)
        => Directory.Move(source, destination);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void MoveFile(string source, string destination)
        => File.Move(source, destination);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FileStream OpenReadFileSteam(string path)
        => File.OpenRead(path);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FileStream OpenFileStream(string path, FileMode mode)
        => File.Open(path, mode);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FileStream OpenFileStream(string path, FileMode mode, FileAccess access)
        => File.Open(path, mode, access);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FileStream OpenFileStream(string path, FileMode mode, FileAccess access, FileShare share)
        => File.Open(path, mode, access, share);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FileStream OpenWriteFileStream(string path)
        => File.OpenWrite(path);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void RemoveFile(string path)
        => File.Delete(path);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void RemoveDirectory(string path, bool recursive = false)
        => Directory.Delete(path, recursive);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] ReadFile(string path)
        => File.ReadAllBytes(path);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ReadTextFile(string path)
        => File.ReadAllText(path);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteFile(string path, byte[] data)
        => File.WriteAllBytes(path, data);

    public static void WriteTextFile(string path, string contents, Encoding? encoding = null, bool append = false)
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
        string path,
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
}