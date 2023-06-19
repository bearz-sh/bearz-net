using System.Runtime.InteropServices;

using Bearz.Secrets;

namespace Bearz.Virtual;

public partial interface IVirtualEnvironment
{
    string Cwd { get; set; }

    bool IsWindows { get; }

    bool IsLinux { get; }

    bool IsMacOS { get; }

    bool Is64BitProcess { get; }

    bool Is64BitOs { get; }

    bool IsUserInteractive { get; set; }

    bool IsPrivilegedProcess { get; }

    bool IsWsl { get; }

    Architecture OsArch { get; }

    Architecture ProcessArch { get; }

    IVirtualEnvironmentPath Path { get; }

    ISecretMasker SecretMasker { get; }

    string User { get; }

    string UserDomain { get; }

    string Host { get; }

    string? this[string key] { get; set; }
}