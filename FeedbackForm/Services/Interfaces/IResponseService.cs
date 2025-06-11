using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FeedbackForm.DTOs;

namespace FeedbackForm.Services.Interfaces
{
    public interface IResponseService
    {
        Task SubmitFormAsync(SubmitFormRequestDto dto);
        Task<List<SubmissionDto>> GetAllSubmissionsAsync();
        Task<SubmissionDto?> GetSubmissionByIdAsync(Guid id);
        Task<bool> DeleteSubmission(Guid Id);
    }
}
