using FeedbackForm.DTOs;
using FeedbackForm.Enum;
using FeedbackForm.Models;
using FeedbackForm.Repositories.Interfaces;
using FeedbackForm.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FeedbackForm.Services.Implementations
{
    public class FormService : IFormService
    {
        private readonly IFormRepository _formRepo;
        private readonly AppSettings _appSettings;
        private readonly ApplicationDbContext _applicationDbContext;
        public FormService(IFormRepository formRepo, IOptions<AppSettings> appSettings, ApplicationDbContext applicationDbContext)
        {
            _formRepo = formRepo;
            _appSettings = appSettings.Value;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Form> CreateFormAsync(Form form)
        {
            if (form != null)
            {
                if (form.Status == FormStatus.Published)
                {
                    form.PublishedOn = DateTime.UtcNow;
                    form.ShareableLink = $"{_appSettings.BaseUrl}/api/form/{form.Id}";
                }
            }
            return await _formRepo.AddAsync(form);
        }

        public async Task<Form> CreateFormWithQuestionsAsync(Form form, List<Question> questions)
        {
            return await _formRepo.AddFormWithQuestionsAsync(form, questions);
        }

        public async Task<Form> GetFormByIdAsync(Guid formId)
        {
            var form = await _formRepo.Query()
                .Where(f => f.Id == formId && !f.isDeleted)
                .Include(f=>f.User)
                .Include(f => f.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync();

            return form;
        }

        public async Task<IEnumerable<Form>> GetAllFormsAsync()
        {
            var forms = await _formRepo.GetAllAsync(
                f => f.Questions,
                f => f.Submissions
            );

            return forms.Where(f => !f.isDeleted);
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
            form.ShareableLink = $"{_appSettings.BaseUrl}/api/form/{form.Id}";
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

        public async Task<bool> EditForm(Guid id, FormUpdateDto formdto)
        {
            var form = _applicationDbContext.Forms
                .Include(f => f.Questions)
                .SingleOrDefault(f => f.Id == id);
            if (form == null)
                return false;
            form.Title = formdto.Title;
            form.Description = formdto.Description;
            foreach (var questionInfo in formdto.Questions)
            {
                var question = form.Questions.SingleOrDefault(q => q.Id == questionInfo.Id);
                if (question != null)
                {
                    question.Text = questionInfo.Text;
                    question.WordLimit = questionInfo.WordLimit;
                    question.IsRequired = questionInfo.IsRequired;
                    question.Order = questionInfo.Order;

                }
            }

            await _applicationDbContext.SaveChangesAsync();
            return true;
        }
     

        public async Task<bool> DeleteForm(Guid formId)
        {
            var form = await _applicationDbContext.Forms.FindAsync(formId);
            if (form == null || form.isDeleted)
                return false;
            form.isDeleted = true;
            form.isModified = DateTime.UtcNow;
            _applicationDbContext.Forms.Update(form);
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }
    }
}
