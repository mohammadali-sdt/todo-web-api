using Entities.LinkModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
namespace TodoAPI.Presentation.Controllers;

[Route("/api")]
[ApiController]
public class RootController : ControllerBase
{

    private readonly LinkGenerator _linkGenerator;

    public RootController(LinkGenerator linkGenerator) => _linkGenerator = linkGenerator;


    [HttpGet(Name = "GetRoot")]
    public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
    {
        if (mediaType.Contains("application/vnd.vinix.apiroot"))
        {
            var links = new List<Link> {
                new Link{
                    Href = _linkGenerator.GetUriByName(HttpContext, nameof(GetRoot), values: new {}),
                    Rel = "self",
                    Method = "GET"
                },
                new Link{
                    Href = _linkGenerator.GetUriByName(HttpContext, "GetUsers", values: new {}),
                    Rel = "users",
                    Method = "GET"
                },
                new Link{
                    Href = _linkGenerator.GetUriByName(HttpContext, "CreateUser", values: new {}),
                    Rel = "create_user",
                    Method = "POST"
                },
            };

            return Ok(links);
        }

        return NoContent();
    }

}
