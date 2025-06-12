using FeedbackForm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FormConfiguration : IEntityTypeConfiguration<Form>
{
    public void Configure(EntityTypeBuilder<Form> builder)
    {

  builder.ToTable("Forms");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(f => f.Description)
               .HasMaxLength(1000);

        builder.Property(f => f.Status)
               .IsRequired();

        builder.Property(f => f.CreatedOn)
               .IsRequired();

        builder.Property(f => f.PublishedOn);

        builder.Property(f => f.ClosedOn);

        builder.Property(f => f.ShareableLink)
               .HasMaxLength(500);

        builder.Property(f => f.isDeleted)
               .IsRequired();

        builder.Property(f => f.isModified)
               .IsRequired();

        builder.Property(f => f.UserId)
               .IsRequired();

        builder.HasMany(f => f.Questions)
               .WithOne(q => q.Form)
               .HasForeignKey(q => q.FormId);

        builder.HasMany(f => f.Submissions)
               .WithOne(s => s.Form)
               .HasForeignKey(s => s.FormId);

        builder.HasOne(f => f.User)
               .WithMany(u => u.Forms)
               .HasForeignKey(f => f.UserId);

    }
}
