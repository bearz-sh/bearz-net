namespace Bearz;

public static class StreamReaderExtensions
{

    public static void PipeTo(this StreamReader sr, Stream stream, bool dispose = false, int bufferSize = 0)
    {
        if (sr is null)
            throw new ArgumentNullException(nameof(sr));

        if (stream is null)
            throw new ArgumentNullException(nameof(stream));

        try
        {
            if (bufferSize < 1)
            {
                sr.BaseStream.CopyTo(stream);
            }
            else
            {
                sr.BaseStream.CopyTo(stream, bufferSize);
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
                sr.Dispose();
        }
    }
}