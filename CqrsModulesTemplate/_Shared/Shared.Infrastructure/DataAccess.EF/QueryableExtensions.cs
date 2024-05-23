using Shared.Application.Queries;

namespace QueryX;

public static class QueryableExtensions
{
    public static IQueryable<TModel> ApplyQuery<TModel, TQueryModel>(
        this IQueryable<TModel> queryable, TQueryModel queryModel, bool applyOrderingAndPaging = true)
        where TModel : class
        where TQueryModel : IQueryModel
    {
        if (applyOrderingAndPaging)
        {
            queryable = queryable.ApplyQuery(queryModel.Filter, queryModel.OrderBy, queryModel.Offset, queryModel.Limit);
        }
        else
        {
            queryable = queryable.ApplyQuery(queryModel.Filter, null, null, null);
        }
        return queryable;
    }

    public static IQueryable<TModel> ApplyOrderingAndPaging<TModel>(
        this IQueryable<TModel> queryable, IQueryModel queryModel)
        where TModel : class
    {
        queryable = queryable.ApplyOrderingAndPaging(queryModel.OrderBy, queryModel.Offset, queryModel.Limit);
        return queryable;
    }
}
