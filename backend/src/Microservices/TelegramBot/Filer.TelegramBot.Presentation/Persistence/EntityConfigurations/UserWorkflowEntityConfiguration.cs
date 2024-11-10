using Filer.TelegramBot.Presentation.UserStates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Filer.TelegramBot.Presentation.Persistence.EntityConfigurations;

public class UserWorkflowEntityConfiguration : IEntityTypeConfiguration<UserWorkflow>
{
    public void Configure(EntityTypeBuilder<UserWorkflow> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder
            .Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(64);
        
        builder
            .Property(x => x.WorkflowPayload)
            .HasColumnType("jsonb")
            .IsRequired();
    }
}