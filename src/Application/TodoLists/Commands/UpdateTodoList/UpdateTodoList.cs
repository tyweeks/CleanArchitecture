using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.TodoLists.Commands.UpdateTodoList;

public record UpdateTodoListCommand : IRequest
{
    public int Id { get; init; }

    public string? Title { get; init; }
}

public class UpdateTodoListCommandHandler : IRequestHandler<UpdateTodoListCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public UpdateTodoListCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoLists
            .Include(l => l.SharedWith)
            .Where(l => l.Id == request.Id && (l.OwnerId == _user.Id || l.SharedWith.Any(a => a.UserId == _user.Id)))
            .FirstOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Title = request.Title;

        await _context.SaveChangesAsync(cancellationToken);

    }
}
