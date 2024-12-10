using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

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

    [HttpGet("{todoId:guid}", Name="TodoById")]
    public IActionResult GetTodoForUser(Guid userId, Guid todoId)
    {
        var todo = _service.TodoService.GetTodo(userId: userId, todoId: todoId, trackChanges:false);

        return Ok(todo);
    }

    [HttpPost]
    public IActionResult CreateTodo(Guid userId, [FromBody] TodoForCreationDto todo)
    {
        if (todo is null) return BadRequest("TodoForCreation object is null.");

        var createdTodo = _service.TodoService.CreateTodo(todo, userId, false);

        return CreatedAtRoute("TodoById", new { userId = userId, todoId = createdTodo.Id }, createdTodo);
    }

    [HttpDelete("{todoId:guid}")]
    public IActionResult DeleteTodo(Guid userId, Guid todoId)
    {
        _service.TodoService.DeleteTodo(userId, todoId, false);

        return NoContent();
    }
    
}