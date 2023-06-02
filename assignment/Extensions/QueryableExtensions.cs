using assignment.Base;
using assignment.Models;
using Microsoft.EntityFrameworkCore;

namespace assignment.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResult<TEntity>> GetPagedAsync<TEntity>(this IQueryable<TEntity> query,
        int page,
        int pageSize) where TEntity : class, IEntity
    {
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var pagedData = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<TEntity>(pagedData, page, pageSize, totalCount, totalPages);
    }
}
