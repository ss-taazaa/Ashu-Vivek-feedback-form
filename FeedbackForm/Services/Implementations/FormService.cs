using FeedbackForm.Models;
using FeedbackForm.Repositories.Interfaces;
using FeedbackForm.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FeedbackForm.Services.Implementations
{
    public class FormService : IFormService
    {
        private readonly IGenericRepository<Form> _formRepo;

        public FormService(IGenericRepository<Form> formRepo)
        {
            _formRepo = formRepo;
        }

        public async Task<Form> CreateFormAsync(Form form)
        {
            form.Status = FormStatus.Draft;
            form.CreatedOn = DateTime.UtcNow;
            return await _formRepo.AddAsync(form);
        }

        public async Task<Form> CreateFormWithQuestionsAsync(Form form, List<Question> questions)
        {
            form.Status = FormStatus.Draft;
            form.CreatedOn = DateTime.UtcNow;
            return await _formRepo.AddFormWithQuestionsAsync(form, questions);
        }

        public async Task<Form> GetFormByIdAsync(Guid formId)
        {
            return await _formRepo.GetSingleAsync(
              predicate: f => f.Id == formId,
              include: f => f
                .Include(f => f.Questions)
                  .ThenInclude(q => q.Options)
                .Include(f => f.Submissions)
            );
        }
        public async Task<IEnumerable<Form>> GetAllFormsAsync()
        {
            return await _formRepo.GetAllAsync(
                f => f.Questions,
                f => f.Submissions
            );
        }

        public async Task<Form> UpdateFormAsync(Form form)
        {
            return await _formRepo.UpdateAsync(form);
        }

        public async Task<bool> PublishFormAsync(Guid formId)
        {
            var form = await _formRepo.GetByIdAsync(formId);
            if (form == null || form.Status != FormStatus.Draft)
                return false;

            form.Status = FormStatus.Published;
            form.PublishedOn = DateTime.UtcNow;
            form.ShareableLink = $"http://localhost:5047/api/forms/{form.Id}";

            await _formRepo.UpdateAsync(form);
            return true;
        }

        public async Task<bool> CloseFormAsync(Guid formId)
        {
            var form = await _formRepo.GetByIdAsync(formId);
            if (form == null || form.Status != FormStatus.Published)
                return false;

            form.Status = FormStatus.Closed;
            form.ClosedOn = DateTime.UtcNow;

            await _formRepo.UpdateAsync(form);
            return true;
        }

        public async Task<bool> UpdateFormQuestionsAsync(Guid formId, List<Question> questions)
        {
            return await _formRepo.UpdateFormQuestionsAsync(formId, questions);
        }
    }
}
