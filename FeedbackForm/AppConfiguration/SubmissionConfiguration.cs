using FeedbackForm.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FeedbackForm.AppConfiguration
{
    public class SubmissionConfiguration : IEntityTypeConfiguration<Submission>
    {
        public void Configure(EntityTypeBuilder<Submission> builder)
        {
            builder.ToTable("Submissions");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.SubmittedOn).IsRequired();

            builder.HasMany(s => s.Answers)
                   .WithOne(a => a.Submission)
                   .HasForeignKey(a => a.SubmissionId);
        }
    }
}
