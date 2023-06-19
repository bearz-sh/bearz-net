namespace Bearz.Virtual.Environment;

public class InMemoryVirtualEnvironmentOptions
{
    public string Cwd { get; set; } = Env.CurrentDirectory;

    public Dictionary<string, string>? Variables { get; set; }
}