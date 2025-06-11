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
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly AppSettings _appSettings;
        private readonly IGenericRepository<Question> _questionRepo;
        private readonly IGenericRepository<Option> _optionRepo;
        

        public FormService(IFormRepository formRepo, IOptions<AppSettings> appSettings)
        {
            _formRepo = formRepo;
            _appSettings = appSettings.Value;
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
                .Where(f => f.Id == formId)
                .Include(f => f.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync();

            return form;
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

        public async Task<bool> UpdateFormQuestionsAsync(Guid formId, List<Question> questions)
        {
            return await _formRepo.UpdateFormQuestionsAsync(formId, questions);
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
