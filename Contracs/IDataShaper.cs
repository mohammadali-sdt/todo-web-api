using System;
using System.Dynamic;

namespace Contracs;

public interface IDataShaper<T>
{
    IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string? fieldsQueryString);

    ExpandoObject ShapeData(T entity, string? fieldsQueryString);
}
