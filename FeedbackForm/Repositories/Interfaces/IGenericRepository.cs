using System.Linq.Expressions;
using FeedbackForm.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedbackForm.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        void Remove(T entity);
        IQueryable<T> GetQueryable();
        Task<T?> GetSingleAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>>? include = null);
        IQueryable<T> Query();
        Task<int> SaveChangesAsync();
    }
}
