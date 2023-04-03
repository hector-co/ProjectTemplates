namespace TplNamespace.Domain.Common;

public record Error(string Code, string Message, Exception? InnerException = null);