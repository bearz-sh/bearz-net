using System.Text;

namespace Bearz;

public static class TextWriterExtensions
{
#if NETLEGACY
    public static void Write(this TextWriter writer, ReadOnlySpan<char> chars)
    {
        var buffer = Arrays.Rent<char>(chars.Length);
        try
        {
            chars.CopyTo(buffer);
            writer.Write(buffer, 0, buffer.Length);
        }
        finally
        {
            Arrays.Return(buffer, true);
        }
    }

    public static Task WriteAsync(this TextWriter writer, ReadOnlyMemory<char> chars, CancellationToken cancellationToken = default)
    {
        var buffer = Arrays.Rent<char>(chars.Length);
        try
        {
            chars.Span.CopyTo(buffer);
            return writer.WriteAsync(buffer, 0, buffer.Length);
        }
        finally
        {
            Arrays.Return(buffer, true);
        }
    }
#endif

    public static void PipeFrom(this TextWriter writer, TextReader reader, bool dispose = false, int bufferSize = -1)
    {
        if (writer is null)
            throw new ArgumentNullException(nameof(writer));

        if (reader is null)
            throw new ArgumentNullException(nameof(reader));

        if (bufferSize < 0)
            bufferSize = 4096;

        var buffer = Arrays.Rent<char>(bufferSize);
        try
        {
            var span = new Span<char>(buffer);
            int read;
            while ((read = reader.Read(span)) > 0)
            {
                writer.Write(span.Slice(0, read));
            }
        }
        catch (Exception ex)
        {
            if (!ex.IsExpectedPipeException())
                throw;
        }
        finally
        {
            Arrays.Return(buffer, true);
            if (dispose)
                writer.Dispose();
        }
    }

    public static void PipeFrom(this TextWriter writer, Stream stream, bool dispose = false, Encoding? encoding = null, int bufferSize = -1, bool leaveOpen = false)
    {
        if (writer is null)
            throw new ArgumentNullException(nameof(writer));

        if (stream is null)
            throw new ArgumentNullException(nameof(stream));

        using var reader = new StreamReader(stream, encoding ?? Encoding.UTF8, true, bufferSize, leaveOpen);
        writer.PipeFrom(reader, dispose, bufferSize);
    }

    public static void PipeFrom(this TextWriter writer, ICollection<string> lines, bool dispose = false)
    {
        if (writer is null)
            throw new ArgumentNullException(nameof(writer));

        if (lines is null)
            throw new ArgumentNullException(nameof(lines));

        try
        {
            foreach (var line in lines)
            {
                writer.WriteLine(line);
            }
        }
        catch (Exception ex)
        {
            if (!ex.IsExpectedPipeException())
                throw;
        }
        finally
        {
            if (dispose)
                writer.Dispose();
        }
    }

    public static void PipeFrom(this TextWriter writer, FileInfo file, bool dispose = false, Encoding? encoding = null, int bufferSize = -1)
    {
        if (writer is null)
            throw new ArgumentNullException(nameof(writer));

        if (file is null)
            throw new ArgumentNullException(nameof(file));

        using var stream = file.OpenRead();
        writer.PipeFrom(stream, dispose, encoding, bufferSize);
    }
}