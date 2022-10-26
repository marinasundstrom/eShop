using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Site.Server.Authentication.Services;
using YourBrand.Customers;

namespace Site.Server.Authentication.Endpoints;

public static class Endpoints
{
    public static WebApplication AddAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/Authentication/Login", Login);
        app.MapPost("/Token/Refresh", RefreshToken);

        return app;
    }

    public static async Task<IResult> Login(ICustomersClient customersClient, ICustomerService customerService, ITokenService tokenService,
        [FromBody] UserForAuthenticationDto userForAuthentication)
    {
        var customer = await customersClient.GetCustomerBySSNAsync(userForAuthentication.SSN);

        if (customer == null)
            return Results.Unauthorized();

        var user = await customerService.GetUserByCustomerId(customer.Id);

        if(user is null)
        {
            user = new User
            {
                CustomerId = customer.Id,
                Email = customer.Email,
                RefreshToken = string.Empty,
                RefreshTokenExpiryTime = DateTime.Now
            };
            await customerService.AddUser(user);
        }

        var signingCredentials = tokenService.GetSigningCredentials();
        var claims = await tokenService.GetClaims(customer);
        var tokenOptions = tokenService.GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        user.RefreshToken = tokenService.GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        await customerService.UpdateUser(user);

        return Results.Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token, RefreshToken = user.RefreshToken });
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

        if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return Results.BadRequest(new AuthResponseDto { IsAuthSuccessful = false, ErrorMessage = "Invalid client request" });

        var signingCredentials = tokenService.GetSigningCredentials();
        var claims = await tokenService.GetClaims(user);
        var tokenOptions = tokenService.GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        user.RefreshToken = tokenService.GenerateRefreshToken();

        await customerService.UpdateUser(user);

        return Results.Ok(new AuthResponseDto { Token = token, RefreshToken = user.RefreshToken, IsAuthSuccessful = true });
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