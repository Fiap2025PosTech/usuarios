using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Usuarios.Domain.Entities.Interfaces;

namespace Usuarios.Infrastructure.Data.Repositories;

public class BaseExpressionService<T>(ApplicationDbContext context) : object() where T : class, IBaseEntity
{
    protected readonly ApplicationDbContext Context = context;

    protected virtual IQueryable<T> BaseQuery(bool tracking = false)
    {
        var condition = (Expression<Func<T, bool>>)(x => x.Removed == false);
        var query = Context.Set<T>().Where(condition);

        if (tracking)
            query = query.AsTracking();
        else
            query = query.AsNoTracking();

        return query.AsQueryable();
    }
}