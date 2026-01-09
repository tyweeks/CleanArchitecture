using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.TodoLists.Commands.ShareTodoList;

public record ShareTodoListCommand : IRequest
{
    public int TodoListId { get; init; }
    public string UserId { get; init; } = null!;
}

public class ShareTodoListCommandHandler : IRequestHandler<ShareTodoListCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public ShareTodoListCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task Handle(ShareTodoListCommand request, CancellationToken cancellationToken)
    {
        var todoList = await _context.TodoLists
            .Where(l => l.Id == request.TodoListId && l.OwnerId == _user.Id)
            .FirstOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.TodoListId, todoList);

        var existingAccess = await _context.TodoListAccesses
            .Where(a => a.TodoListId == request.TodoListId && a.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingAccess == null)
        {
            var access = new TodoListAccess
            {
                TodoListId = request.TodoListId,
                UserId = request.UserId
            };

            _context.TodoListAccesses.Add(access);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
