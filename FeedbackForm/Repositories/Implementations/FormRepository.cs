using FeedbackForm.DTOs;
using FeedbackForm.Enum;
using FeedbackForm.Models;
using FeedbackForm.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FeedbackForm.Repositories.Implementations
{
    public class FormRepository : GenericRepository<Form>, IFormRepository
    {
        public FormRepository(ApplicationDbContext context) : base(context) { }

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

            var existingQuestionIds = updatedQuestions
                .Where(q => q.Id != Guid.Empty)
                .Select(q => q.Id)
                .ToList();

            var questionsToRemove = form.Questions
                .Where(q => !existingQuestionIds.Contains(q.Id))
                .ToList();

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

        public async Task<(List<Form> Items, int TotalCount)> GetFilteredFormsAsync(FormFilterDto filter)
        {
            var query = _context.Forms
                .Include(f => f.User)
                .Include(f => f.Questions)
                .Include(f => f.Submissions)
                .AsQueryable();
            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(f => f.Title.Contains(filter.Title));
            if (filter.Status.HasValue)
                query = query.Where(f => f.Status == (FormStatus)filter.Status.Value);
            var totalCount = await query.CountAsync();
            var forms = await query
                .OrderByDescending(f => f.CreatedOn)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
            return (forms, totalCount);
        }




        public IQueryable<Form> Query()
        {
            return _context.Forms;
        }


    }
}
