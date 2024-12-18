using System;
using Contracs;
using Entities.LinkModels;
using Entities.Models;
using Shared.DataTransferObjects;
using Microsoft.Net.Http.Headers;

namespace TodoAPI.Utility;

public class UserLinks : IUserLinks
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IDataShaper<UserDto> _dataShaper;

    public UserLinks(LinkGenerator linkGenerator, IDataShaper<UserDto> dataShaper)
    {
        _linkGenerator = linkGenerator;
        _dataShaper = dataShaper;
    }
    public LinkResponse TryGenerateLinks(IEnumerable<UserDto> usersDto, string fields, HttpContext httpContext)
    {
        var shapedUsers = ShapeData(usersDto, fields);

        if (ShouldGenerateLinks(httpContext))
        {
            return ReturnLinkdedUsers(usersDto, fields, httpContext, shapedUsers);
        }

        return ReturnShapedUsers(shapedUsers);
    }

    private List<Entity> ShapeData(IEnumerable<UserDto> usersDto, string fields)
    {
        return _dataShaper.ShapeData(usersDto, fields)
        .Select(e => e.Entity)
        .ToList();
    }

    private bool ShouldGenerateLinks(HttpContext httpContext)
    {
        System.Console.WriteLine();
        var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];

        return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
    }

    private LinkResponse ReturnShapedUsers(List<Entity> shapedUsers) =>
        new LinkResponse { ShapedEntities = shapedUsers };

    private LinkResponse ReturnLinkdedUsers(IEnumerable<UserDto> usersDto, string fields, HttpContext httpContext, List<Entity> shapedUsers)
    {
        var usersDtoList = usersDto.ToList();

        for (var index = 0; index < usersDtoList.Count; index++)
        {
            var userLinks = CreateLinksForUser(httpContext, usersDtoList[index].Id, fields);
            shapedUsers[index].Add("Links", userLinks);
        }

        var userCollection = new LinkCollectionWrapper<Entity>(shapedUsers);
        var linkdedUsers = CreateLinksForUsers(httpContext, userCollection);

        return new LinkResponse { HasLinks = true, LinkedEntities = linkdedUsers };
    }

    private List<Link> CreateLinksForUser(HttpContext httpContext, Guid userId, string fields = "")
    {
        var links = new List<Link>{
            new Link(_linkGenerator.GetUriByAction(httpContext, "GetUser", values: new {userId}), "self", "GET"),
            new Link(_linkGenerator.GetUriByAction(httpContext, "DeleteUser", values: new {userId}), "delete_user", "DELETE"),
            new Link(_linkGenerator.GetUriByAction(httpContext, "UpdateUser", values: new {userId}), "update_user", "PUT"),
        };

        return links;
    }

    private LinkCollectionWrapper<Entity> CreateLinksForUsers(HttpContext httpContext, LinkCollectionWrapper<Entity> usersWrapper)
    {
        usersWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext, "GetUsers", values: new { }), "self", "GET"));

        return usersWrapper;
    }
}
