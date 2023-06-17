using System.Buffers;
using System.Text;

namespace Bearz;

public static class TextReaderExtensions
{
#if NETLEGACY
    public static int Read(this TextReader reader, Span<char> chars)
    {
        var buffer = Arrays.Rent<char>(chars.Length);
        try
        {
            var read = reader.Read(buffer, 0, buffer.Length);
            if (read > 0)
                buffer.AsSpan(0, read).CopyTo(chars);

            return read;
        }
        finally
        {
            Arrays.Return(buffer, true);
        }
    }

    public static Task<int> ReadAsync(this TextReader reader, Memory<char> chars, CancellationToken cancellationToken = default)
    {
        var buffer = Arrays.Rent<char>(chars.Length);
        try
        {
            var read = reader.Read(buffer, 0, buffer.Length);
            if (read > 0)
                buffer.AsSpan(0, read).CopyTo(chars.Span);

            return Task.FromResult(read);
        }
        finally
        {
            Arrays.Return(buffer, true);
        }
    }

#endif

    public static void PipeTo(
        this TextReader reader,
        TextWriter writer,
        bool dispose = false,
        int bufferSize = -1)
    {
        if (reader is null)
            throw new ArgumentNullException(nameof(reader));

        if (writer is null)
            throw new ArgumentNullException(nameof(writer));

        if (bufferSize == -1)
            bufferSize = 4096;

        var buffer = Arrays.Rent<char>(bufferSize);
        try
        {
            int read;
            var span = new Span<char>(buffer);

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
                reader.Dispose();
        }
    }

    public static void PipeTo(
        this TextReader reader,
        Stream stream,
        bool dispose = false,
        Encoding? encoding = null,
        int bufferSize = -1,
        bool leaveOpen = false)
    {
        if (reader is null)
            throw new ArgumentNullException(nameof(reader));

        if (stream is null)
            throw new ArgumentNullException(nameof(stream));

        try
        {
            using var sw = new StreamWriter(stream, encoding, bufferSize, leaveOpen);
            reader.PipeTo(sw, dispose);
        }
        finally
        {
            if (dispose)
                reader.Dispose();
        }
    }

    public static void PipeTo(
        this TextReader reader,
        FileInfo file,
        bool dispose = false,
        Encoding? encoding = null,
        int bufferSize = -1)
    {
        if (reader is null)
            throw new ArgumentNullException(nameof(reader));

        if (file is null)
            throw new ArgumentNullException(nameof(file));

        using var stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.Read);
        reader.PipeTo(stream, dispose, encoding, bufferSize, false);
    }

    public static void PipeTo(
        this TextReader reader,
        ICollection<string> lines,
        bool dispose = false)
    {
        if (reader is null)
        {
            throw new ArgumentNullException(nameof(reader));
        }

        if (lines is null)
        {
            throw new ArgumentNullException(nameof(lines));
        }

        try
        {
            while (reader.ReadLine() is { } line)
            {
                lines.Add(line);
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
                reader.Dispose();
        }
    }

    public static Task PipeToAsync(
        this TextReader reader,
        FileInfo file,
        bool dispose = false,
        Encoding? encoding = null,
        int bufferSize = -1,
        CancellationToken cancellationToken = default)
    {
        if (reader is null)
            throw new ArgumentNullException(nameof(reader));

        if (file is null)
            throw new ArgumentNullException(nameof(file));

        return InnerPipeToAsync(reader, file, dispose, encoding, bufferSize, cancellationToken);
    }

    public static Task PipeToAsync(
        this TextReader reader,
        ICollection<string> lines,
        bool dispose = false,
        CancellationToken cancellationToken = default)
    {
        if (reader is null)
        {
            throw new ArgumentNullException(nameof(reader));
        }

        if (lines is null)
        {
            throw new ArgumentNullException(nameof(lines));
        }

        return InnerPipeToAsync(reader, lines, dispose, cancellationToken);
    }

    public static Task PipeToAsync(
        this TextReader reader,
        TextWriter writer,
        bool dispose = false,
        int bufferSize = -1,
        CancellationToken cancellationToken = default)
    {
        if (reader is null)
            throw new ArgumentNullException(nameof(reader));

        if (writer is null)
            throw new ArgumentNullException(nameof(writer));

        return InnerPipeToAsync(reader, writer, dispose, bufferSize, cancellationToken);
    }

    public static Task PipeToAsync(
        this TextReader reader,
        Stream stream,
        bool dispose = false,
        Encoding? encoding = null,
        int bufferSize = -1,
        bool leaveOpen = false,
        CancellationToken cancellationToken = default)
    {
        if (reader is null)
            throw new ArgumentNullException(nameof(reader));

        if (stream is null)
            throw new ArgumentNullException(nameof(stream));

        return InnerPipeToAsync(reader, stream, dispose, encoding, bufferSize, leaveOpen, cancellationToken);
    }

    private static async Task InnerPipeToAsync(
        TextReader reader,
        TextWriter writer,
        bool dispose,
        int bufferSize,
        CancellationToken cancellationToken)
    {
        if (bufferSize == -1)
            bufferSize = 4096;

        char[] buffer = Arrays.Rent<char>(bufferSize);
        try
        {
            int read;
            var memory = new Memory<char>(buffer);

            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            while ((read = await reader.ReadAsync(memory, cancellationToken)) > 0)
            {
                await writer.WriteAsync(memory.Slice(0, read), cancellationToken);
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
                reader.Dispose();
        }
    }

    private static async Task InnerPipeToAsync(
        TextReader reader,
        Stream stream,
        bool dispose = false,
        Encoding? encoding = null,
        int bufferSize = -1,
        bool leaveOpen = false,
        CancellationToken cancellationToken = default)
    {
#if NETLEGACY
        using var sw = new StreamWriter(stream, encoding ?? Encoding.UTF8, bufferSize, leaveOpen);
#else
        await using var sw = new StreamWriter(stream, encoding, bufferSize, leaveOpen);
#endif
        await reader.PipeToAsync(sw, dispose, bufferSize, cancellationToken);
    }

    private static async Task InnerPipeToAsync(
        TextReader reader,
        FileInfo file,
        bool dispose,
        Encoding? encoding,
        int bufferSize,
        CancellationToken cancellationToken)
    {
#if NETLEGACY
        using var stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.Read);
#else
        await using var stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.Read);
#endif
        await reader.PipeToAsync(stream, dispose, encoding, bufferSize, false, cancellationToken);
    }

    private static async Task InnerPipeToAsync(
        TextReader reader,
        ICollection<string> lines,
        bool dispose,
        CancellationToken cancellationToken)
    {
        try
        {
#if !NET7_0_OR_GREATER
            while (await reader.ReadLineAsync().ConfigureAwait(false) is { } line)
            {
                lines.Add(line);
            }
#else
            while (await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false) is { } line)
            {
                lines.Add(line);
            }
#endif
        }
        catch (Exception ex)
        {
            if (!ex.IsExpectedPipeException())
                throw;
        }
        finally
        {
            if (dispose)
                reader.Dispose();
        }
    }
}