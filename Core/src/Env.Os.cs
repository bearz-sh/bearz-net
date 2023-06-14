using System;

namespace Bearz;

public static partial class Env
{
    public static readonly Lazy<bool> IsElevatedUser = new Lazy<bool>(() =>
    {
        if (IsWindows)
        {
            return Interop.Shell32.IsUserAnAdmin();
        }

        return Interop.Sys.GetEUid() == 0;
    });

    private static readonly Lazy<bool> isWindows = new Lazy<bool>(() => OperatingSystem.IsWindows());

    private static readonly Lazy<bool> isLinux = new Lazy<bool>(() => OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD());

    private static readonly Lazy<bool> isMacOS = new Lazy<bool>(() => OperatingSystem.IsMacOS());

    public static bool IsWindows => isWindows.Value;

    public static bool IsLinux => isLinux.Value;

    public static bool IsMacOS => isMacOS.Value;

    public static bool Is64BitOs => Environment.Is64BitOperatingSystem;

    public static bool IsPrivilegedProcess => IsElevatedUser.Value;
}