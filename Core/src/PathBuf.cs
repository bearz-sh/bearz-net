using System.Text;

namespace Bearz;

public sealed class PathBuf
{
    private readonly StringBuilder buffer;

    public PathBuf()
    {
        this.buffer = new StringBuilder();
    }

    public PathBuf(PathStr path)
    {
        this.buffer = new StringBuilder();
        this.buffer.Append(path.ToArray());
    }

    public int Length => this.buffer.Length;

    public bool IsEmpty => this.buffer.Length == 0;

    public char this[int index]
    {
        get => this.buffer[index];
        set => this.buffer[index] = value;
    }

    public static implicit operator PathBuf(PathStr path)
    {
        return new PathBuf(path);
    }

    public static implicit operator PathBuf(string path)
    {
        return new PathBuf(path);
    }

    public static implicit operator PathBuf(StringBuilder path)
    {
        return new PathBuf(path);
    }

    public static implicit operator PathBuf(ReadOnlySpan<char> path)
    {
        return new PathBuf(path);
    }

    public static PathBuf New(ReadOnlySpan<char> path)
    {
        return new PathBuf(path);
    }

    public static PathBuf New(StringBuilder path)
    {
        return new PathBuf(path);
    }

    public static PathBuf New(PathStr path)
    {
        return new PathBuf(path);
    }

    public void CopyTo(char[] path, int index)
    {
        this.buffer.CopyTo(0, path, index, this.buffer.Length);
    }

    public void CopyTo(Span<char> path)
    {
        this.buffer.CopyTo(0, path, this.buffer.Length);
    }

    public void CopyTo(Span<char> path, int index)
    {
        this.buffer.CopyTo(index, path, this.buffer.Length);
    }

    public PathBuf Join(ReadOnlySpan<char> path)
    {
        if (this.buffer.Length > 0)
            this.buffer.Append(System.IO.Path.DirectorySeparatorChar);

        this.buffer.Append(path);
        return this;
    }

    public PathBuf Push(ReadOnlySpan<char> path)
    {
        if (System.IO.Path.IsPathFullyQualified(path))
        {
            this.buffer.Clear();
            this.buffer.Append(path);
        }

        if (this.buffer.Length > 0)
            this.buffer.Append(System.IO.Path.DirectorySeparatorChar);

        int i = 0;
        while (i < path.Length)
        {
            var c = path[i];
            if (c is '\\' or '/')
            {
                i++;
                continue;
            }

            break;
        }

        if (i > 0)
            path = path.Slice(i);

        this.buffer.Append(path);
        return this;
    }

    public PathBuf Pop()
    {
        var path = this.buffer.ToString();
        var index = path.LastIndexOfAny(new[] { '\\', '/' });
        if (index < 0)
        {
            this.buffer.Clear();
            return this;
        }

        this.buffer.Remove(index, path.Length - index);
        return this;
    }

    public char[] ToArray()
    {
        var span = new char[this.buffer.Length];
        this.buffer.CopyTo(0, span, 0, this.buffer.Length);
        return span;
    }

    public PathStr ToPathStr()
    {
        return new PathStr(this.buffer);
    }

    public ReadOnlySpan<char> ToReadOnlySpan()
    {
        return this.ToArray();
    }

    public override string ToString()
    {
        return this.buffer.ToString();
    }
}