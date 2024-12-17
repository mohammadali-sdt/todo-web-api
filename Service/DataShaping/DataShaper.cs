using System.Dynamic;
using System.Linq;
using System.Reflection;
using Contracs;

namespace Service.DataShaping;

public class DataShaper<T> : IDataShaper<T>
{

    private readonly PropertyInfo[] _properties;

    public DataShaper()
    {
        _properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string? fieldsQueryString)
    {
        IEnumerable<PropertyInfo> requiredProperties = GetRequiredProperties(fieldsQueryString);

        return FetchData(entities, requiredProperties);

    }

    public ExpandoObject ShapeData(T entity, string? fieldsQueryString)
    {
        IEnumerable<PropertyInfo> requiredProperties = GetRequiredProperties(fieldsQueryString);

        return FetchEntity(entity, requiredProperties);
    }

    private IEnumerable<PropertyInfo> GetRequiredProperties(string? fieldsQueryString)
    {
        var requiredProperties = new List<PropertyInfo>();

        var fields = string.IsNullOrWhiteSpace(fieldsQueryString) ? [] : fieldsQueryString.Split([','], StringSplitOptions.RemoveEmptyEntries);

        var objectProperties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToList();

        foreach (var field in fields)
        {
            if (string.IsNullOrWhiteSpace(field)) continue;

            var property = objectProperties.SingleOrDefault(pi => pi.Name.Equals(field, StringComparison.InvariantCultureIgnoreCase));

            if (property is null) continue;

            requiredProperties.Add(property);
        }

        if (requiredProperties.Count == 0)
            requiredProperties = _properties.ToList();

        return requiredProperties;

    }

    private IEnumerable<ExpandoObject> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
    {
        var resultData = new List<ExpandoObject>();

        foreach (var entity in entities)
        {
            var resultObject = FetchEntity(entity, requiredProperties);
            resultData.Add(resultObject);
        }

        return resultData;
    }

    private ExpandoObject FetchEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)
    {
        var resultObject = new ExpandoObject();

        foreach (var property in requiredProperties)
        {
            var entityPropertyValue = property.GetValue(entity);
            resultObject.TryAdd(property.Name, entityPropertyValue);
        }

        return resultObject;
    }
}
