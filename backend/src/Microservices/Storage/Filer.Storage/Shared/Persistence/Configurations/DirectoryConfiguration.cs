using Filer.Storage.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Filer.Storage.Shared.Persistence.Configurations;

public sealed class DirectoryConfiguration : IEntityTypeConfiguration<DirectoryObject>
{
    public void Configure(EntityTypeBuilder<DirectoryObject> builder)
    {
        builder.ToTable("directories");

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .HasMaxLength(256)
            .ValueGeneratedNever();
        
        builder
            .Property(x => x.Path)
            .HasMaxLength(1024)
            .ValueGeneratedNever();
        
        builder
            .Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(128)
            .ValueGeneratedNever();
        
        builder
            .HasOne(x => x.ParentDirectory)
            .WithMany(x => x.SubDirectories)
            .HasForeignKey(x => x.ParentDirectoryId)
            .IsRequired(false);
        
        builder
            .Property(x => x.CreatedAt)
            .IsRequired()
            .ValueGeneratedNever();
        
        builder
            .Property(x => x.ModifiedAt)
            .IsRequired(false)
            .ValueGeneratedNever();
    }
}