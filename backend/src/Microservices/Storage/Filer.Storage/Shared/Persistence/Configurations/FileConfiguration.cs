using Filer.Storage.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Filer.Storage.Shared.Persistence.Configurations;

public sealed class FileConfiguration : IEntityTypeConfiguration<FileObject>
{
    public void Configure(EntityTypeBuilder<FileObject> builder)
    {
        builder.ToTable("files");

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(x => x.Name)
            .HasMaxLength(256)
            .ValueGeneratedNever();
        
        builder
            .Property(x => x.Path)
            .HasMaxLength(1024)
            .ValueGeneratedNever();
        
        builder
            .Property(x => x.Extension)
            .HasMaxLength(256)
            .ValueGeneratedNever();
        
        builder
            .Property(x => x.Size)
            .IsRequired()
            .ValueGeneratedNever();
        
        builder
            .Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(128)
            .ValueGeneratedNever();
        
        builder
            .Property(x => x.Created)
            .IsRequired()
            .ValueGeneratedNever();
        
        builder
            .Property(x => x.Modified)
            .IsRequired(false)
            .ValueGeneratedNever();
        
        builder
            .HasOne(x => x.ParentDirectory)
            .WithMany(x => x.Files)
            .HasForeignKey(x => x.ParentDirectoryId)
            .IsRequired(false);
    }
}