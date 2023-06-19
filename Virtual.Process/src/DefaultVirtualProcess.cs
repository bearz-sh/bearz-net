using System.Collections.Concurrent;
using System.Diagnostics;

using Bearz.Extra.Strings;

namespace Bearz.Virtual.Process;

public class DefaultVirtualProcess : IVirtualProcess
{
    private readonly ConcurrentDictionary<string, string> executableLocationCache = new();

    public int Id => Ps.Id;

    public IReadOnlyList<string> Argv => Ps.Argv;

    public string CommandLine => Ps.CommandLine;

    public System.Diagnostics.Process Current => Ps.Current;

    public int ExitCode
    {
        get => Ps.ExitCode;
        set => Ps.ExitCode = value;
    }

    public Ps Create(string fileName)
        => new(fileName);

    public Ps Create(string fileName, PsStartInfo startInfo)
        => new(fileName, startInfo);

    public Ps Create(string fileName, PsArgs args)
        => new(fileName, new PsStartInfo() { Args = args });

    public string? Which(string command, IEnumerable<string>? prependPaths = null, bool useCache = false)
    {
        // https://github.com/actions/runner/blob/592ce1b230985aea359acdf6ed4ee84307bbedc1/src/Runner.Sdk/Util/WhichUtil.cs
        if (string.IsNullOrWhiteSpace(command))
            throw new ArgumentNullException(nameof(command));

        var rootName = Path.GetFileNameWithoutExtension(command);
        if (useCache && this.executableLocationCache.TryGetValue(rootName, out var location))
            return location;

#if NETLEGACY
        if (Path.IsPathRooted(command) && File.Exists(command))
        {
            this.executableLocationCache[command] = command;
            this.executableLocationCache[rootName] = command;

            return command;
        }
#else
        if (Path.IsPathFullyQualified(command) && File.Exists(command))
        {
            this.executableLocationCache[command] = command;
            this.executableLocationCache[rootName] = command;

            return command;
        }
#endif

        var pathSegments = new List<string>();
        if (prependPaths is not null)
            pathSegments.AddRange(prependPaths);

        pathSegments.AddRange(Env.SplitPath());

        for (var i = 0; i < pathSegments.Count; i++)
        {
            pathSegments[i] = Env.Expand(pathSegments[i]);
        }

        foreach (var pathSegment in pathSegments)
        {
            if (string.IsNullOrEmpty(pathSegment) || !System.IO.Directory.Exists(pathSegment))
                continue;

            IEnumerable<string> matches = Array.Empty<string>();
            if (Env.IsWindows)
            {
                var pathExt = Env.Get("PATHEXT");
                if (pathExt.IsNullOrWhiteSpace())
                {
                    // XP's system default value for PATHEXT system variable
                    pathExt = ".com;.exe;.bat;.cmd;.vbs;.vbe;.js;.jse;.wsf;.wsh";
                }

                var pathExtSegments = pathExt.ToLowerInvariant()
                                             .Split(
                                                 new[] { ";", },
                                                 StringSplitOptions.RemoveEmptyEntries);

                // if command already has an extension.
                if (pathExtSegments.Any(command.EndsWithIgnoreCase))
                {
                    try
                    {
                        matches = System.IO.Directory.EnumerateFiles(pathSegment, command);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }

                    var result = matches.FirstOrDefault();
                    if (result is null)
                        continue;

                    this.executableLocationCache[rootName] = result;
                    return result;
                }
                else
                {
                    string searchPattern = $"{command}.*";
                    try
                    {
                        matches = System.IO.Directory.EnumerateFiles(pathSegment, searchPattern);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }

                    var expandedPath = Path.Combine(pathSegment, command);

                    foreach (var match in matches)
                    {
                        foreach (var ext in pathExtSegments)
                        {
                            var fullPath = expandedPath + ext;
                            if (!match.Equals(fullPath, StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }

                            this.executableLocationCache[rootName] = fullPath;
                            return fullPath;
                        }
                    }
                }
            }
            else
            {
                try
                {
                    matches = System.IO.Directory.EnumerateFiles(pathSegment, command);
                }
                catch (UnauthorizedAccessException ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

                var result = matches.FirstOrDefault();
                if (result is null)
                    continue;

                this.executableLocationCache[rootName] = result;
                return result;
            }
        }

        return null;
    }

    public PsOutput Run(string fileName)
    {
        var ps = new Ps(fileName)
            .WithStdio(Stdio.Inherit);

        var r = ps.Output();
        if (r.IsError)
            r.Throw();

        return r;
    }

    public PsOutput Run(string fileName, PsArgs args)
    {
        var ps = new Ps(fileName, new PsStartInfo() { Args = args })
            .WithStdio(Stdio.Inherit);
        var r = ps.Output();
        if (r.IsError)
            r.Throw();

        return r;
    }

    public PsOutput Run(string fileName, PsStartInfo startInfo)
    {
        var ps = new Ps(fileName, startInfo)
            .WithStdio(Stdio.Inherit);
        var r = ps.Output();
        if (r.IsError)
            r.Throw();

        return r;
    }

    public async Task<PsOutput> RunAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var ps = new Ps(fileName)
            .WithStdio(Stdio.Inherit);
        var r = await ps.OutputAsync(cancellationToken).ConfigureAwait(false);
        if (r.IsError)
            r.Throw();

        return r;
    }

    public async Task<PsOutput> RunAsync(string fileName, PsArgs args, CancellationToken cancellationToken = default)
    {
        var ps = new Ps(fileName, new PsStartInfo() { Args = args })
            .WithStdio(Stdio.Inherit);
        var r = await ps.OutputAsync(cancellationToken).ConfigureAwait(false);
        if (r.IsError)
            r.Throw();

        return r;
    }

    public async Task<PsOutput> RunAsync(string fileName, PsStartInfo startInfo, CancellationToken cancellationToken = default)
    {
        var ps = new Ps(fileName, startInfo)
            .WithStdio(Stdio.Inherit);
        var r = await ps.OutputAsync(cancellationToken).ConfigureAwait(false);
        if (r.IsError)
            r.Throw();

        return r;
    }
}