using Market.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        T GetById(int id);
        ValueTask<T> GetByIdAsync(int id);
        IEnumerable<T> List();
        IAsyncEnumerable<T> ListAsync();
        List<T> ToList();
        Task<List<T>> ToListAsync();
        IEnumerable<T> List(Expression<Func<T, bool>> predicate);
        IAsyncEnumerable<T> ListAsync(Expression<Func<T, bool>> predicate);
        void Insert(T entity);
        Task InsertAsync(T entity);
        void Update(T entity);
        Task UpdateAsync(T entity);
        void Delete(T entity);
        Task DeleteAsync(T entity);
    }
}
