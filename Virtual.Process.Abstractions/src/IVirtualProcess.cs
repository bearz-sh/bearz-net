namespace Bearz.Virtual;

public interface IVirtualProcess
{
    int Id { get; }

    IReadOnlyList<string> Argv { get; }

    string CommandLine { get; }

    System.Diagnostics.Process Current { get; }

    int ExitCode { get; set; }

    Ps Create(string fileName);

    Ps Create(string fileName, PsStartInfo startInfo);

    Ps Create(string fileName, PsArgs args);

    string? Which(string command, IEnumerable<string>? prependPaths = null, bool useCache = false);

    PsOutput Run(string fileName);

    PsOutput Run(string fileName, PsArgs args);

    PsOutput Run(string fileName, PsStartInfo startInfo);

    Task<PsOutput> RunAsync(string fileName, CancellationToken cancellationToken = default);

    Task<PsOutput> RunAsync(string fileName, PsArgs args, CancellationToken cancellationToken = default);

    Task<PsOutput> RunAsync(string fileName, PsStartInfo startInfo, CancellationToken cancellationToken = default);
}