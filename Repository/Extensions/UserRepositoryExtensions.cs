using System.Linq.Expressions;
using Entities.Models;

namespace Repository.Extensions;

public static class UserRepositoryExtensions
{
    public static IQueryable<User> FilterUsers(this IQueryable<User> users, int minAge, int maxAge)
    {
        return users.Where(u => u.Age >= minAge && u.Age <= maxAge);
    }

    public static IQueryable<User> Search(this IQueryable<User> users, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) return users;
        var cleanSearchTerm = searchTerm.Trim().ToLower();
        return users.Where(u =>
            u.Username.Trim().ToLower().Contains(searchTerm) ||
            u.Name.Trim().ToLower().Contains(searchTerm) ||
            u.Email.Trim().ToLower().Contains(searchTerm)
            );
    }
}