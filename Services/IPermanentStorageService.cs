using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Piller.Services
{
    public interface IPermanentStorageService
    {
        Task<T> GetAsync<T> (int id) where T : new();
        Task SaveAsync<T> (T entity);
        Task<List<T>> List<T> (Expression<Func<T, bool>> predicate = null) where T : new();
    }
}
