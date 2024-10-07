using Carter;
using Domain.Interfaces;
using Shared.Dtos.Requests;
using Shared.Dtos.Responses;

namespace API.Endpoints;

public class ProfileEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/profile").WithTags("Profile");

        group.MapPost("register", async (RegisterProfileRequest request, IProfileService profileService, ICookieService cookieService) =>
        {
            var tokens = await profileService.CreateAsync(request);
            cookieService.IssueAuthCookies(tokens);

            return TypedResults.Ok(new AuthenticatedResponse
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                IdToken = tokens.IdToken
            });
        })
        .Produces<AuthenticatedResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .AllowAnonymous();

        group.MapPost("login", async (LoginProfileRequest request, IProfileService profileService, ICookieService cookieService) =>
        {
            var tokens = await profileService.LoginAsync(request);
            cookieService.IssueAuthCookies(tokens);

            return TypedResults.Ok(new AuthenticatedResponse
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                IdToken = tokens.IdToken
            });
        })
        .Produces<AuthenticatedResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .AllowAnonymous();

        group.MapPost("logout", (ICookieService cookieService) =>
        {
            cookieService.DeleteAuthCookies();

            return TypedResults.NoContent();
        }).RequireAuthorization();
    }
}