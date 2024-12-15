using Microsoft.AspNetCore.JsonPatch;
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
    public async Task<IActionResult> GetTodosForUser(Guid userId)
    {
        var todos = await _service.TodoService.GetAllTodosAsync(userId, trackChanges: false);

        return Ok(todos);
    }

    [HttpGet("{todoId:guid}", Name = "TodoById")]
    public async Task<IActionResult> GetTodoForUser(Guid userId, Guid todoId)
    {
        var todo = await _service.TodoService.GetTodoAsync(userId: userId, todoId: todoId, trackChanges: false);

        return Ok(todo);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodo(Guid userId, [FromBody] TodoForCreationDto todo)
    {
        if (todo is null) return BadRequest("TodoForCreation object is null.");

        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }

        var createdTodo = await _service.TodoService.CreateTodoAsync(todo, userId, false);

        return CreatedAtRoute("TodoById", new { userId = userId, todoId = createdTodo.Id }, createdTodo);
    }

    [HttpDelete("{todoId:guid}")]
    public async Task<IActionResult> DeleteTodo(Guid userId, Guid todoId)
    {
        await _service.TodoService.DeleteTodoAsync(userId, todoId, false);

        return NoContent();
    }

    [HttpPut("{todoId:guid}")]
    public async Task<IActionResult> UpdateTodo(Guid userId, Guid todoId, [FromBody] TodoForUpdateDto todo)
    {

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);
        
        await _service.TodoService.UpdateTodoAsync(todo, userId, todoId, userTrackChanges: false, todoTrackChanges: true);

        return NoContent();
    }

    [HttpPatch("{todoId:guid}")]
    public async Task<IActionResult> PartiallyUpdateTodo(Guid userId, Guid todoId,
        [FromBody] JsonPatchDocument<TodoForUpdateDto>? todoPatch)
    {
        if (todoPatch is null) 
            return BadRequest("todoPatch object is null.");

        var result = await _service.TodoService.PartiallyUpdateTodoAsync(userId, todoId, true, false);
        
        todoPatch.ApplyTo(result.todoForUpdateDto, ModelState);

        TryValidateModel(result.todoForUpdateDto);
        
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);
            
        
        await _service.TodoService.SavePartiallyUpdateTodoAsync(result.todoForUpdateDto, result.todoEntity);

        return NoContent();

    }
}