using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace TodoAPI.Presentation.Controllers;


[Route("api/users/{userId:guid}/todos")]
[ApiController]
public class TodosController : ControllerBase
{
    private readonly IServiceManager _service;
    public TodosController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetTodosForUser(Guid userId)
    {
        var todos = _service.TodoService.GetAllTodos(userId, trackChanges: false);
        
        return Ok(todos);
    }

    [HttpGet("{todoId:guid}")]
    public IActionResult GetTodoForUser(Guid userId, Guid todoId)
    {
        var todo = _service.TodoService.GetTodo(userId: userId, todoId: todoId, trackChanges:false);

        return Ok(todo);
    }
    
}