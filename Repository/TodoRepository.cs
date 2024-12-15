using System;
using Contracs;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class TodoRepository: RepositoryBase<Todo>, ITodoRepository
{
    public TodoRepository(RepositoryContext repositoryContext) : base(repositoryContext){}

    public async Task<IEnumerable<Todo>> GetAllTodosAsync(Guid userId, bool trackChanges) =>
        await FindByCondition(t => t.UserId.Equals(userId), trackChanges).ToListAsync();

    public async Task<Todo> GetTodoAsync(Guid userId, Guid todoId, bool trackChanges) =>
        await FindByCondition(t => t.UserId.Equals(userId) && t.Id.Equals(todoId), trackChanges).SingleOrDefaultAsync();
    public void CreateTodo(Guid userId, Todo todo)
    {
        todo.UserId = userId;
        Create(todo);
    }

    public void DeleteTodo(Todo todo) => Delete(todo);
}
