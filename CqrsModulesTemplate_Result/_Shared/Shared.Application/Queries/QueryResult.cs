using System.Text.Json.Serialization;

namespace Shared.Application.Queries;

public class QueryResult<TData>
{
    public QueryResult(TData? data, int? totalCount = default)
    {
        Data = data;
        TotalCount = totalCount;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TData? Data { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object>? Meta { get; private set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TotalCount { get; }

    public QueryResult<TData> AddMeta(string key, object value)
    {
        Meta ??= [];

        Meta.Add(key, value);

        return this;
    }
}