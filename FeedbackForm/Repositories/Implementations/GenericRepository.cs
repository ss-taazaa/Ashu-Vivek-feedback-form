﻿using FeedbackForm.Data;
using FeedbackForm.Models;
using FeedbackForm.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FeedbackForm.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes)
        {
                IQueryable<T> query = _dbSet;

                foreach (var include in includes)
                query = query.Include(include);

                return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
                query = query.Include(include);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.Where(predicate);
            foreach (var include in includes)
                query = query.Include(include);

            return await query.ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            _context.SaveChanges();
        }


        public async Task<Form> AddFormWithQuestionsAsync(Form form, List<Question> questions)
        {
            form.Questions = questions;
            await _context.Forms.AddAsync(form);
            await _context.SaveChangesAsync();
            return form;
        }

        public async Task<bool> UpdateFormQuestionsAsync(Guid formId, List<Question> updatedQuestions)
        {
            var form = await _context.Forms
                .Include(f => f.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(f => f.Id == formId);

            if (form == null)
                return false;

            var existingQuestionIds = updatedQuestions.Where(q => q.Id != Guid.Empty).Select(q => q.Id).ToList();
            var questionsToRemove = form.Questions.Where(q => !existingQuestionIds.Contains(q.Id)).ToList();

            _context.Questions.RemoveRange(questionsToRemove);

            foreach (var updatedQuestion in updatedQuestions)
            {
                var existing = form.Questions.FirstOrDefault(q => q.Id == updatedQuestion.Id);
                if (existing != null)
                {
                    existing.Text = updatedQuestion.Text;
                    existing.Type = updatedQuestion.Type;
                    existing.WordLimit = updatedQuestion.WordLimit;
                    existing.IsRequired = updatedQuestion.IsRequired;
                    existing.Order = updatedQuestion.Order;


                }
                else
                {
                    updatedQuestion.FormId = formId;
                    form.Questions.Add(updatedQuestion);
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }





        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public IQueryable<T> GetQueryable()
        {
            return _context.Set<T>();
        }

        public async Task<T?> GetSingleAsync(
                Expression<Func<T, bool>> predicate,
                Func<IQueryable<T>, IQueryable<T>>? include = null)
                {
                    IQueryable<T> query = _dbSet;

                    if (include != null)
                    query = include(query);

            return await query.FirstOrDefaultAsync(predicate);
        }




    }
}
