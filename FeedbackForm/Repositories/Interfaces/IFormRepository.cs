﻿using FeedbackForm.DTOs;
using FeedbackForm.Models;

namespace FeedbackForm.Repositories.Interfaces
{
    public interface IFormRepository : IGenericRepository<Form>
    {
        Task<Form> AddFormWithQuestionsAsync(Form form, List<Question> questions);
        Task<bool> UpdateFormQuestionsAsync(Guid formId, List<Question> questions);
        IQueryable<Form> Query();
         Task<(List<Form> Items, int TotalCount)> GetFilteredFormsAsync(FormFilterDto filter);


    }
}
