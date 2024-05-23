namespace Shared.Application.Commands;

public record CommandBase<TValue> : RequestBase<TValue>, ICommand<TValue>;
