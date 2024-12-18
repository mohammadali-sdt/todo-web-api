using System;
using System.Dynamic;
using Entities.Models;

namespace Contracs;

public interface IDataShaper<T>
{
    IEnumerable<Entity> ShapeData(IEnumerable<T> entities, string? fieldsQueryString);

    Entity ShapeData(T entity, string? fieldsQueryString);
}
