using System;
using Contracs;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared.RequestFeature;

namespace Repository;

public class TodoRepository: RepositoryBase<Todo>, ITodoRepository
{
    public TodoRepository(RepositoryContext repositoryContext) : base(repositoryContext){}

    public async Task<PagedList<Todo>> GetAllTodosAsync(Guid userId, TodoParameters todoParameters , bool trackChanges)  {
        
        // great for small amount of data
        var todos = await FindByCondition(t => t.UserId.Equals(userId), trackChanges)
            .OrderBy(t => t.Title)
            .Search(todoParameters.SearchTerm)
            .ToListAsync();
        return PagedList<Todo>
            .ToPagedList(todos, todoParameters.PageNumber, todoParameters.PageSize);

        // if we have millions of rows...
        // var todos = await FindByCondition(t => t.UserId.Equals(userId), trackChanges)
        //     .OrderBy(t => t.Title)
        //     .Skip((todoParameters.PageNumber - 1) * todoParameters.PageSize)
        //     .Take(todoParameters.PageSize)
        //     .ToListAsync();
        //
        // var count = await FindByCondition(t => t.UserId.Equals(userId), trackChanges).CountAsync();
        //
        // return new PagedList<Todo>(todos, count, todoParameters.PageNumber, todoParameters.PageSize);
    }
        

    public async Task<Todo> GetTodoAsync(Guid userId, Guid todoId, bool trackChanges) =>
        await FindByCondition(t => t.UserId.Equals(userId) && t.Id.Equals(todoId), trackChanges).SingleOrDefaultAsync();
    public void CreateTodo(Guid userId, Todo todo)
    {
        todo.UserId = userId;
        Create(todo);
    }

    public void DeleteTodo(Todo todo) => Delete(todo);
}
