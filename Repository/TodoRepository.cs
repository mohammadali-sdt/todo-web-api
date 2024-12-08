using System;
using Contracs;
using Entities.Exceptions;
using Entities.Models;

namespace Repository;

public class TodoRepository: RepositoryBase<Todo>, ITodoRepository
{
    public TodoRepository(RepositoryContext repositoryContext) : base(repositoryContext){}

    public IEnumerable<Todo> GetAllTodos(Guid userId, bool trackChanges) =>
        FindByCondition(t => t.UserId.Equals(userId), trackChanges);

    public Todo GetTodo(Guid userId, Guid todoId, bool trackChanges) =>
        FindByCondition(t => t.UserId.Equals(userId) && t.Id.Equals(todoId), trackChanges).SingleOrDefault();
}
