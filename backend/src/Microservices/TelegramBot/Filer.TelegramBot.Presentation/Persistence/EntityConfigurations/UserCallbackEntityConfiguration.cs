using Filer.TelegramBot.Presentation.UserStates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Filer.TelegramBot.Presentation.Persistence.EntityConfigurations;

public sealed class UserCallbackEntityConfiguration : IEntityTypeConfiguration<UserCallback>
{
    public void Configure(EntityTypeBuilder<UserCallback> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder
            .Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(64);
        
        builder
            .Property(x => x.CallbackPayload)
            .HasColumnType("jsonb")
            .IsRequired();
    }
}