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
    public class ResponseService : IResponseService
    {


        private readonly IGenericRepository<Form> _formRepo;
        private readonly IGenericRepository<Submission> _submissionRepo;

        public ResponseService(
            IGenericRepository<Form> formRepo,
            IGenericRepository<Submission> submissionRepo)
        {
            _formRepo = formRepo;
            _submissionRepo = submissionRepo;
        }



        public async Task SubmitFormAsync(SubmitFormRequestDto dto)
        {
            var form = await _formRepo.GetSingleAsync(
                f => f.ShareableLink == dto.ShareableLink,
                include: f => f.Include(x => x.Questions).ThenInclude(q => q.Options)
            );

            if (form == null)
                throw new Exception("Form not found.");

            var validOptionIds = form.Questions
                .SelectMany(q => q.Options)
                .Select(o => o.Id)
                .ToHashSet(); // Use a set for fast lookup

            var submission = new Submission(dto, form.Id);

            foreach (var answer in submission.Answers)
            {
                foreach (var answerOption in answer.AnswerOptions)
                {
                    if (!validOptionIds.Contains(answerOption.OptionId))
                        throw new Exception($"Invalid Option ID: {answerOption.OptionId}");

                    answerOption.Option = null; // Ensure EF doesn't try to insert new Option
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





    }
}
