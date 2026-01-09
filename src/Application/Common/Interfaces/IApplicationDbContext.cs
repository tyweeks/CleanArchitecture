using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<TodoListAccess> TodoListAccesses { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
