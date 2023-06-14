using System.Diagnostics;

namespace Bearz;

public interface IPsCapture
{
    void OnStart(Process process);

    void WriteLine(string line);

    void OnExit();
}