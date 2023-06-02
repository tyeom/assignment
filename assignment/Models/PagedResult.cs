using assignment.Base;

namespace assignment.Models;

public record PagedResult<TEntity>(IEnumerable<TEntity> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages)
    where TEntity : IEntity;