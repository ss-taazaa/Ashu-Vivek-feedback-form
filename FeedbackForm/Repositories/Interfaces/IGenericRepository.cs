using System.Linq.Expressions;
using FeedbackForm.Models;

namespace FeedbackForm.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes);
        //Eager loading loads the main entity and its related data at the same time, using a single query (with JOINs).
        // here the  params Expression<Func<T, object>>[] includes is used in this function to tell that is will eager loading
        //

        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);


        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);

        Task<T> UpdateAsync(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        //Task<Form> AddFormWithQuestionsAsync(Form form, List<Question> questions);
        
        //Task<bool> UpdateFormQuestionsAsync(Guid formId, List<Question> questions);
        IQueryable<T> GetQueryable();
        Task<T?> GetSingleAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>>? include = null);
       
    }
}
