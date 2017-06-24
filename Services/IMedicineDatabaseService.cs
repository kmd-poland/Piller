using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Piller.Services
{
    public interface IMedicineDatabaseService
    {
        Task<T> GetAsync<T>(long KodEAN) where T : new();
    }
}
