using assignment.Extensions;
using assignment.Models;
using assignment.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Principal;

namespace assignment.Base;

public abstract class EfCoreRepository<TEntity, TDBContext> : IRepository<TEntity>
    where TEntity : class, IEntity
    where TDBContext : DbContext
{
    private readonly ILoggerService _logger;
    private readonly TDBContext _context;
    public EfCoreRepository(ILoggerService logger, TDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<TEntity> Add(TEntity entity)
    {
        _logger.LogInfo($"데이터 추가! - {entity.Id}");

        _context.Set<TEntity>().Add(entity);
        await _context.SaveChangesAsync();

        _logger.LogInfo("데이터 완료!");
        return entity;
    }

    public async Task<TEntity?> Delete(long id)
    {
        _logger.LogInfo($"데이터 삭제! - {id}");

        var findEntity = await _context.Set<TEntity>().FindAsync(id);
        if (findEntity is null)
            return null;

        _context.Set<TEntity>().Remove(findEntity);
        await _context.SaveChangesAsync();

        _logger.LogInfo($"데이터 완료");
        return findEntity;
    }

    public async Task<TEntity> Delete(TEntity entity)
    {
        _logger.LogInfo($"데이터 삭제! - {entity.Id}");

        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();

        _logger.LogInfo($"데이터 완료");
        return entity;
    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public async Task<PagedResult<TEntity>> GetByPaged(int page, int pageSize)
    {
        return await _context.Set<TEntity>().GetPagedAsync<TEntity>(page, pageSize);
    }

    public async Task<IEnumerable<TEntity>?> FindAll(Expression<Func<TEntity, bool>> predicate)
    {
        var findEntity = await _context.Set<TEntity>().Where(predicate).ToListAsync();
        return findEntity as IEnumerable<TEntity>;
    }

    public async Task<TEntity> Update(TEntity entity)
    {
        _logger.LogInfo($"데이터 수정! - {entity.Id}");

        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        _logger.LogInfo($"데이터 완료");
        return entity;
    }
}