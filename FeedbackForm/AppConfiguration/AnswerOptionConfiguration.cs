using FeedbackForm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FeedbackForm.AppConfiguration
{
    public class AnswerOptionConfiguration : IEntityTypeConfiguration<AnswerOption>
    {
        public void Configure(EntityTypeBuilder<AnswerOption> builder)
        {
            builder.ToTable("AnswerOptions");

            builder.HasKey(ao => ao.Id);

            builder.HasOne(ao => ao.Answer)
                   .WithMany(a => a.AnswerOptions)
                   .HasForeignKey(ao => ao.AnswerId);

            builder.HasOne(ao => ao.Option)
                   .WithMany()
                   .HasForeignKey(ao => ao.OptId);
        }
    }
}
