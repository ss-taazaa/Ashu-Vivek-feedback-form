using FeedbackForm.DTOs;
using FeedbackForm.Models;
using System;

namespace FeedbackForm.Services.Interfaces
{
    public interface IFormService
    {
        Task<Form> CreateFormAsync(Form form);
        Task<Form> CreateFormWithQuestionsAsync(Form form, List<Question> questions);
        Task<Form> GetFormByIdAsync(Guid formId);
        Task<IEnumerable<Form>> GetAllFormsAsync();
        Task<Form> UpdateFormAsync(Form form);
        Task<bool> PublishFormAsync(Guid formId);
        Task<bool> CloseFormAsync(Guid formId);
        Task<bool> EditForm(Guid formId, FormUpdateDto form);
        Task<bool> DeleteForm(Guid id);
    }
}
