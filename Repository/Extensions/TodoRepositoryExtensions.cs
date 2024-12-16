using Entities.Models;

namespace Repository.Extensions;

public static class TodoRepositoryExtensions
{
    public static IQueryable<Todo> Search(this IQueryable<Todo> todos, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) return todos;

        var cleanSearchTerm = searchTerm.Trim().ToLower();

        return todos.Where(t => t.Title.Trim().ToLower().Contains(searchTerm));
    }
}