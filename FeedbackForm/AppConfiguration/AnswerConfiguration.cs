using FeedbackForm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FeedbackForm.AppConfiguration
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("Answers");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.TextAnswer)
                   .HasMaxLength(1000);
            builder.Property(a => a.TextAnswer)
                   .HasColumnName("TextAnswer")
                   .IsRequired(false);
            builder.Property(a => a.RatingValue);

            builder.Property(a => a.Ranking);

            builder.HasOne(a => a.Question)
                    .WithMany(q => q.Answers)
                    .HasForeignKey(a => a.QuestionId);

            builder.HasOne(a => a.Submission)
                   .WithMany(s => s.Answers)
                   .HasForeignKey(a => a.SubmissionId);

            builder.HasMany(a => a.AnswerOptions)
                   .WithOne(ao => ao.Answer)
                   .HasForeignKey(ao => ao.AnswerId);

            builder.Property(a => a.TextAnswer)
       .HasMaxLength(1000)
       .IsRequired(false);
        }
    }
}
