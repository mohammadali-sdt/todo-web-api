using System.Reflection;
using System.Text;
using Entities.Models;
using System.Linq.Dynamic.Core;
using Repository.Extensions.Utility;

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
            u.UserName.Trim().ToLower().Contains(searchTerm) ||
            u.Name.Trim().ToLower().Contains(searchTerm) ||
            u.Email.Trim().ToLower().Contains(searchTerm)
            );
    }

    public static IQueryable<User> Sort(this IQueryable<User> users, string? orderQueryParams)
    {
        if (string.IsNullOrWhiteSpace(orderQueryParams))
        {
            return users.OrderBy(u => u.Name);
        }

        var queryBuilder = new OrderQueryStringBuilder<User>().BuildOrderQueryString(orderQueryParams);

        return string.IsNullOrWhiteSpace(queryBuilder) ? users.OrderBy(u => u.Name) : users.OrderBy(queryBuilder);
    }
}