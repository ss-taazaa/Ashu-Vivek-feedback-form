using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FeedbackForm.DTOs;
using FeedbackForm.Models;
using FeedbackForm.Repositories.Interfaces;
using FeedbackForm.Services.Interfaces;

namespace FeedbackForm.Services.Implementations

{

    public class ResponseService(IGenericRepository<Form> _formRepo, IGenericRepository<Submission> _submissionRepo,ApplicationDbContext _applicationDbContext) : IResponseService

    {
        public async Task SubmitFormAsync(SubmitFormRequestDto dto)
        {
            var form = await _formRepo.GetSingleAsync(
                f => f.ShareableLink == dto.ShareableLink,
                include: f => f.Include(x => x.Questions).ThenInclude(q => q.Options)
            );

            if (form == null)
                throw new Exception("Form not found.");


            if (form.Status == Enum.FormStatus.Closed)
            {
                throw new Exception("Can not submit the form because it has been closed now");
            }
            var validOptionIds = form.Questions
                .SelectMany(q => q.Options)
                .Select(o => o.Id)
                .ToHashSet(); 
            var submission = new Submission(dto, form.Id);
            foreach (var answer in submission.Answers)
            {
                if (answer.AnswerOptions != null)
                {
                    foreach (var answerOption in answer.AnswerOptions)
                    {
                        if (!validOptionIds.Contains(answerOption.OptionId))
                            throw new Exception($"Invalid Option ID: {answerOption.OptionId}");
                        answerOption.Option = null;
                    }
                }
            }
            try
            {
                await _submissionRepo.AddAsync(submission);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error saving submission: " + ex.InnerException?.Message, ex);
            }
        }

        public async Task<List<SubmissionDto>> GetAllSubmissionsAsync()
        {
            var submissions = await _submissionRepo.GetAllAsync();

            var existingSubmissions = submissions.Where(s => !s.isDeleted);
            return existingSubmissions.Select(s => new SubmissionDto(s)).ToList();
        }

        public async Task<SubmissionDto> GetSubmissionByIdAsync(Guid id)
        {
            var submission = await _submissionRepo.GetSingleAsync(
                s => s.Id == id,
                include: s => s
                    .Include(x => x.Answers)
                        .ThenInclude(a => a.Question)
                    .Include(x => x.Answers)
                        .ThenInclude(a => a.AnswerOptions)
                            .ThenInclude(ao => ao.Option)
            );
            if (submission == null ||  submission.isDeleted)

                return null;

            return new SubmissionDto(submission);
        }
        public async Task<bool> DeleteSubmission(Guid Id)
        {
            var submission = await _applicationDbContext.Submissions.FindAsync(Id);
            if (submission == null || submission.isDeleted)
                return false;

            submission.isDeleted = true;
            submission.isModified = DateTime.UtcNow;
            _applicationDbContext.Submissions.Update(submission);
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }

    }
}
