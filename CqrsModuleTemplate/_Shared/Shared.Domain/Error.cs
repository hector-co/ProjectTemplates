namespace Shared.Domain;

public record Error(string Code, string Message, Exception? InnerException = null);