namespace Shared.Application.Queries;

public interface IQueryModel
{
    string? Filter { get; }
    string? OrderBy { get; }
    int? Offset { get; }
    int? Limit { get; }
}
