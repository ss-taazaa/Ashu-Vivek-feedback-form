using FeedbackForm.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FeedbackForm.AppConfiguration
{
    public class OptionConfiguration : IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> builder)
        {
            builder.ToTable("Options");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Text).IsRequired().HasMaxLength(200);
        }
    }
}
