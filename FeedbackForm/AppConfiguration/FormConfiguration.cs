using FeedbackForm.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FeedbackForm.AppConfiguration
{
    public class FormConfiguration : IEntityTypeConfiguration<Form>
    {
        public void Configure(EntityTypeBuilder<Form> builder)
        {
            builder.ToTable("Forms");
            builder.HasKey(f => f.Id);
            builder.Property(f => f.Title).IsRequired().HasMaxLength(200);
            builder.Property(f => f.Description).HasMaxLength(1000);
            builder.Property(f => f.Status).IsRequired();

            builder.HasMany(f => f.Questions)
                   .WithOne(q => q.Form)
                   .HasForeignKey(q => q.FormId);

            builder.HasMany(f => f.Submissions)
                   .WithOne(s => s.Form)
                   .HasForeignKey(s => s.FormId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
