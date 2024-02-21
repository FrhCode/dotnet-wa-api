namespace Whatsapp.Api.AppException;

public class BadRequestException : Exception
{
	public IDictionary<string, string> Errors { get; }

	public BadRequestException() : this("Bad Request", new Dictionary<string, string>()) { }

	public BadRequestException(IDictionary<string, string> errors) : this("Bad Request", errors) { }

	public BadRequestException(string message) : this(message, new Dictionary<string, string>()) { }

	public BadRequestException(string message, IDictionary<string, string> errors) : base(message)
	{
		Errors = errors;
	}
}