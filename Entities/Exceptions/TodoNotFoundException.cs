namespace Entities.Exceptions;

public class TodoNotFoundException : NotFoundException
{
    public TodoNotFoundException(Guid todoId) : base($"Todo with id : {todoId} not exists in database.") {}
}