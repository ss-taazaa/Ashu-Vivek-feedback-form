using System;
using FeedbackForm.Models;

namespace FeedbackForm.Services.Interfaces
{
    public interface IFormService
    {
        // int this task keyword is used to define a asynchronous function although async is not wrritten here but we will define it later in the implementation
        
        Task<Form> CreateFormAsync(Form form);
        //this function is used to create the form without questions when user wants t create and add it later

        Task<Form> CreateFormWithQuestionsAsync(Form form, List<Question> questions);

        //this function is used for creating the form with questions initially

        Task<Form> GetFormByIdAsync(Guid formId);
        // this function is used to get the form by using the form Id
        Task<IEnumerable<Form>> GetAllFormsAsync();
        //this function isused to get all the forms
        //main purpose of IEnumberable here is keeps the abstraction of the database does not show from where the data is coming
        //the query is built and stored, but not executed until it’s needed(usually when looping).
        //No, IEnumerable<T> does not always support deferred execution. it supports only with .Where() .Select() .Skip
        //It does not support when you Using any of these forces eager execution: .ToList() .ToArray() .Count() (forces enumeration to count) .First(), .Last(), .Any(), etc.

        Task<Form> UpdateFormAsync(Form form);

        //this function is used to update the form
        Task<bool> PublishFormAsync(Guid formId);
        //this function is used to change the status fo the form to publish

        Task<bool> CloseFormAsync(Guid formId);
        //this function is used to change the status fo the form to publish
        Task<bool> UpdateFormQuestionsAsync(Guid formId, List<Question> questions);
        //this function is used to change the status fo the form to publish
    }
}
