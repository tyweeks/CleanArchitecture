using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class TodoListAccessConfiguration : IEntityTypeConfiguration<TodoListAccess>
{
    public void Configure(EntityTypeBuilder<TodoListAccess> builder)
    {
        builder.Property(t => t.UserId)
            .HasMaxLength(450)
            .IsRequired();

        builder.HasOne(t => t.TodoList)
            .WithMany(t => t.SharedWith)
            .HasForeignKey(t => t.TodoListId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(t => new { t.TodoListId, t.UserId })
            .IsUnique();
    }
}
