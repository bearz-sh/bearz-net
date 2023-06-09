using System;

namespace Bearz;

public static partial class Env
{
    private static readonly Lazy<string[]> argv = new Lazy<string[]>(() => Environment.GetCommandLineArgs());

    public static bool Is64BitProcess => Environment.Is64BitProcess;

    public static string? ProcessPath => Environment.ProcessPath;

    public static int ProcessId => Environment.ProcessId;

    public static IReadOnlyList<string> Argv => argv.Value;

    public static string User => Environment.UserName;

    public static string UserDomain => Environment.UserDomainName;

    public static string Host => Environment.MachineName;

    public static string NewLine => Environment.NewLine;

    public static string CurrentDirectory
    {
        get => Environment.CurrentDirectory;
        set => Environment.CurrentDirectory = value;
    }

    public static bool UserInteractive => Environment.UserInteractive;

    public static string Home => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
}