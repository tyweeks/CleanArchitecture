namespace CleanArchitecture.Domain.Entities;

public class TodoListAccess : BaseEntity
{
    public int TodoListId { get; set; }

    public string UserId { get; set; } = null!;

    public TodoList TodoList { get; set; } = null!;
}
