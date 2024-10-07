using Carter;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Dtos.Requests;
using Shared.Dtos.Responses;

namespace API.Endpoints;

public class ProfileEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/profile").WithTags("Profile");

        group.MapPost("register",
            async Task<Results<Ok<AuthenticatedResponse>, BadRequest>>
            (RegisterProfileRequest request, IProfileService profileService, ICookieService cookieService) =>
        {
            var tokens = await profileService.CreateAsync(request);
            cookieService.IssueAuthCookies(tokens);

            return TypedResults.Ok(new AuthenticatedResponse
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                IdToken = tokens.IdToken
            });
        }).AllowAnonymous();

        group.MapPost("login",
            async Task<Results<Ok<AuthenticatedResponse>, BadRequest, NotFound>>
            (LoginProfileRequest request, IProfileService profileService, ICookieService cookieService) =>
        {
            var tokens = await profileService.LoginAsync(request);
            cookieService.IssueAuthCookies(tokens);

            return TypedResults.Ok(new AuthenticatedResponse
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                IdToken = tokens.IdToken
            });
        }).AllowAnonymous();

        group.MapPost("logout", (ICookieService cookieService) =>
        {
            cookieService.DeleteAuthCookies();

            return TypedResults.NoContent();
        }).RequireAuthorization();
    }
}