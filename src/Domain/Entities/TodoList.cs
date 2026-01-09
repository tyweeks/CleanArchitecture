namespace CleanArchitecture.Domain.Entities;

public class TodoList : BaseAuditableEntity
{
    public string? Title { get; set; }

    public Colour Colour { get; set; } = Colour.White;

    public string? OwnerId { get; set; }

    public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();

    public IList<TodoListAccess> SharedWith { get; private set; } = new List<TodoListAccess>();
}
