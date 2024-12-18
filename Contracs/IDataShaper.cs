using System;
using System.Dynamic;
using Entities.Models;

namespace Contracs;

public interface IDataShaper<T>
{
    IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string? fieldsQueryString);

    ShapedEntity ShapeData(T entity, string? fieldsQueryString);
}
