using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.TodoLists.Commands.RevokeTodoListAccess;

public record RevokeTodoListAccessCommand : IRequest
{
    public int TodoListId { get; init; }
    public string UserId { get; init; } = null!;
}

public class RevokeTodoListAccessCommandHandler : IRequestHandler<RevokeTodoListAccessCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public RevokeTodoListAccessCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task Handle(RevokeTodoListAccessCommand request, CancellationToken cancellationToken)
    {
        var todoList = await _context.TodoLists
            .Where(l => l.Id == request.TodoListId && l.OwnerId == _user.Id)
            .FirstOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.TodoListId, todoList);

        var access = await _context.TodoListAccesses
            .Where(a => a.TodoListId == request.TodoListId && a.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (access != null)
        {
            _context.TodoListAccesses.Remove(access);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
