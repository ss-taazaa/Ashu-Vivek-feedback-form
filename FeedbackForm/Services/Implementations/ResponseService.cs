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

            var submission = new Submission
            {
                Id = Guid.NewGuid(),
                FormId = form.Id,
                SubmittedOn = DateTime.UtcNow,
                RespondentName = dto.RespondentName,
                RespondentEmail = dto.RespondentEmail,
                Answers = new List<Answer>()
            };

            foreach (var answerDto in dto.Answers)
            {
                var answer = new Answer
                {
                    Id = Guid.NewGuid(),
                    QuestionId = answerDto.QuestionId,
                    SubmissionId = submission.Id,
                    TextAnswer = answerDto.TextAnswer ?? string.Empty,
                    RatingValue = answerDto.RatingValue,
                    Ranking = answerDto.Ranking,
                    AnswerOptions = new List<AnswerOption>()
                };

                if (answerDto.AnswerOptions != null)
                {
                    foreach (var optionDto in answerDto.AnswerOptions)
                    {
                        answer.AnswerOptions.Add(new AnswerOption
                        {
                            Id = Guid.NewGuid(),
                            OptId = optionDto.OptionId
                        });
                    }
                }

                submission.Answers.Add(answer);
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
