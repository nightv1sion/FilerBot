using System.Text.Json;
using Filer.TelegramBot.Presentation.UserStates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Filer.TelegramBot.Presentation.Persistence.EntityConfigurations;

public class UserStateEntityConfiguration : IEntityTypeConfiguration<UserState>
{
    public void Configure(EntityTypeBuilder<UserState> builder)
    {
        builder.ToTable("user_states");
        
        builder.HasKey(x => x.Id);
        
        builder
            .Property(x => x.UserId)
            .ValueGeneratedNever()
            .HasMaxLength(64)
            .IsRequired();

        builder
            .HasOne(x => x.CurrentWorkflow)
            .WithMany()
            .HasForeignKey(x => x.CurrentWorkflowId)
            .IsRequired(false);

        builder
            .HasIndex(x => x.UserId)
            .IsUnique();
    }
}