using Carter;
using Domain.Interfaces;

namespace API.Endpoints
{
    public class TokenEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/token/refresh", async (ICookieService cookieService) =>
            {
                await cookieService.RefreshAuthCookiesAsync();

                return TypedResults.NoContent();
            })
             .RequireAuthorization()
             .WithTags("Token");
        }
    }
}