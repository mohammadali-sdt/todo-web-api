using Microsoft.AspNetCore.Http;
using Shared.RequestFeature;

namespace Entities.LinkModels;

public record class LinkParameters(UserParameters userParameters, HttpContext HttpContext);
