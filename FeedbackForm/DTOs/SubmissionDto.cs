﻿namespace FeedbackForm.DTOs
{
    public class SubmissionDto
    {
        public Guid Id { get; set; }
        public Guid FormId { get; set; }
        public DateTime SubmittedOn { get; set; }
        public string RespondentId { get; set; }
        public List<AnswerDto> Answers { get; set; }
    }
}
