namespace Bearz;

public class Error : IError
{
    public Error(string? message, IInnerError? innerError = null)
    {
        this.Message = message ?? "Unknown error";
        this.InnerError = innerError;
    }

    public string Message { get;  }

    public string? Code { get; set; }

    public string? Target { get; set; }

    public IInnerError? InnerError { get; }

    public override string ToString()
    {
        return this.Message;
    }
}