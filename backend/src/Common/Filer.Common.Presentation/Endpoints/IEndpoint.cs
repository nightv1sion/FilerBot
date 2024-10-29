using Microsoft.AspNetCore.Routing;

namespace Filer.Common.Presentation.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
