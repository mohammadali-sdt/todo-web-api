using System.Reflection;
using System.Text;

namespace Repository.Extensions.Utility;

public class OrderQueryStringBuilder<T>
{
    public string BuildOrderQueryString(string queryStringParams)
    {
        var queryPropertyNames = queryStringParams.ToLower().Trim().Split(',', StringSplitOptions.RemoveEmptyEntries);

        var queryBuilder = new StringBuilder();

        var entityProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(pi => pi.Name.Trim().ToLower())
            .ToList();

        foreach (var queryParam in queryPropertyNames)
        {
            if (string.IsNullOrWhiteSpace(queryParam)) continue;
            
            var param = queryParam.Split([' '])[0].ToLower().Trim();
            
            if (string.IsNullOrWhiteSpace(param) || !entityProperties.Contains(param)) continue;

            var direction = queryParam.Contains(" desc") ? "descending" : "ascending";
            queryBuilder.Append($"{param} {direction},");
        }
        var orderQueryString = queryBuilder.ToString().TrimEnd(',', ' ');

        return orderQueryString;
    }
}