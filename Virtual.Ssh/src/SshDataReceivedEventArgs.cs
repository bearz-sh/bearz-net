namespace Bearz.Virtual.Ssh;

public class SshDataReceivedEventArgs : EventArgs
{
    public SshDataReceivedEventArgs(string? data)
    {
        this.Data = data;
    }

    public string? Data { get; }
}