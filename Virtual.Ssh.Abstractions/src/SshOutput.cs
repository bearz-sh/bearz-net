namespace Bearz.Virtual.Ssh;

public class SshOutput
{
    public string Text { get; set; } = string.Empty;

    public IReadOnlyList<string> StdOut { get; set; } = Array.Empty<string>();

    public IReadOnlyList<string> StdErr { get; set; } = Array.Empty<string>();

    public DateTime? StartedAt { get; set; }

    public DateTime? ExitedAt { get; set; }

    public int ExitCode { get; set; }
}