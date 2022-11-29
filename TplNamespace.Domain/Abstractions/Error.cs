namespace TplNamespace.Domain.Abstractions;

public record Error(string Code, string Message, Exception? InnerException = null);