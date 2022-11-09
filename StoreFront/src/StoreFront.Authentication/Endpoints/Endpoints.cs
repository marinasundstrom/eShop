using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YourBrand.StoreFront.Authentication.Services;
using YourBrand.Customers;

namespace YourBrand.StoreFront.Authentication.Endpoints;

public static class Endpoints
{
    public static WebApplication AddAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/Authentication/Login", Login)
            .Produces<AuthResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags("Authentication");

        app.MapPost("/Token/Refresh", RefreshToken)
            .WithTags("Token");

        return app;
    }

    public static async Task<IResult> Login(ICustomersClient customersClient, ICustomerService customerService, ITokenService tokenService,
        [FromBody] UserForAuthenticationDto userForAuthentication)
    {

        CustomerDto? customer = null;

        try
        {
            customer = await customersClient.GetCustomerBySSNAsync(userForAuthentication.SSN);
        }
        catch (ApiException exc) when (exc.StatusCode == 204)
        {
            return Results.NotFound();
        }

        var user = await customerService.GetUserByCustomerId(customer.Id);

        if(user is null)
        {
            user = new User
            {
                CustomerId = customer.Id,
                Email = customer.Email
            };
            await customerService.AddUser(user);
        }

        var signingCredentials = tokenService.GetSigningCredentials();
        var claims = await tokenService.GetClaims(customer);
        var tokenOptions = tokenService.GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        var refreshToken = new UserRefreshToken();

        refreshToken.RefreshToken = tokenService.GenerateRefreshToken();
        refreshToken.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        user.RefreshTokens.Add(refreshToken);

        await customerService.UpdateUser(user);

        return Results.Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token, RefreshToken = refreshToken.RefreshToken });
    }

    public static async Task<IResult> RefreshToken(ICustomerService customerService, ITokenService tokenService,
        [FromBody] RefreshTokenDto tokenDto)
    {
        if (tokenDto is null)
        {
            return Results.BadRequest(new AuthResponseDto { IsAuthSuccessful = false, ErrorMessage = "Invalid client request" });
        }

        var principal = tokenService.GetPrincipalFromExpiredToken(tokenDto.Token);

        var customerId = int.Parse(principal.FindFirst("CustomerId")!.Value);

        var user = await customerService.GetUserByCustomerId(customerId);

        if(user is null)
        {
            return Results.BadRequest(new AuthResponseDto { IsAuthSuccessful = false, ErrorMessage = "Invalid client request" });
        }

        var refreshToken = user.RefreshTokens.FirstOrDefault(x => x.RefreshToken == tokenDto.RefreshToken);

        if (refreshToken is null || refreshToken.RefreshToken != tokenDto.RefreshToken || refreshToken.RefreshTokenExpiryTime <= DateTime.Now)
            return Results.BadRequest(new AuthResponseDto { IsAuthSuccessful = false, ErrorMessage = "Invalid client request" });

        var signingCredentials = tokenService.GetSigningCredentials();
        var claims = await tokenService.GetClaims(user);
        var tokenOptions = tokenService.GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        refreshToken.RefreshToken = tokenService.GenerateRefreshToken();

        await customerService.UpdateUser(user);

        return Results.Ok(new AuthResponseDto { Token = token, RefreshToken = refreshToken.RefreshToken, IsAuthSuccessful = true });
    }

    /*
    public static async Task<IResult> Login(UserManager<User> userManager, ITokenService tokenService,
        UserForAuthenticationDto userForAuthentication)
    {
        var user = await _userManager.FindByNameAsync(userForAuthentication.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
            return Results.Unauthorized();

        var signingCredentials = tokenService.GetSigningCredentials();
        var claims = await tokenService.GetClaims(user);
        var tokenOptions = tokenService.GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        user.RefreshToken = tokenService.GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        await userManager.UpdateAsync(user);

        return Results.Ok(new AuthResponseDto { Token = token });
    }

    public static async Task<IResult> Refresh(UserManager<User> userManager, ITokenService tokenService,
        RefreshTokenDto tokenDto)
    {
        if (tokenDto is null)
        {
            return Results.BadRequest(new AuthResponseDto { IsAuthSuccessful = false, ErrorMessage = "Invalid client request" });
        }

        var principal = tokenService.GetPrincipalFromExpiredToken(tokenDto.Token);
        var username = principal.Identity.Name;
        var user = await _userManager.FindByEmailAsync(username);
        if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return Results.BadRequest(new AuthResponseDto { IsAuthSuccessful = false, ErrorMessage = "Invalid client request" });

        var signingCredentials = tokenService.GetSigningCredentials();
        var claims = await tokenService.GetClaims(user);
        var tokenOptions = tokenService.GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        user.RefreshToken = tokenService.GenerateRefreshToken();

        await _userManager.UpdateAsync(user);

        return Results.Ok(new AuthResponseDto { Token = token, RefreshToken = user.RefreshToken, IsAuthSuccessful = true });
    }
    */
}

public class RefreshTokenDto
{
    public string Token { get; set; }

    public string RefreshToken { get; set; }
}

public class UserForAuthenticationDto
{
    public string SSN { get; set; }

    /*public string Email { get; set; }

    public string Password { get; set; } */
}

public class AuthResponseDto
{
    public string Token { get; set; }

    public string RefreshToken { get; set; }

    public bool IsAuthSuccessful { get; internal set; }

    public string ErrorMessage { get; internal set; }
}