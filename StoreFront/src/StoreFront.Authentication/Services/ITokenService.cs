using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using YourBrand.Customers;

namespace YourBrand.StoreFront.Authentication.Services;

public interface ITokenService
{
    SigningCredentials GetSigningCredentials();

    Task<List<Claim>> GetClaims(User user);

    Task<List<Claim>> GetClaims(CustomerDto customer);

    JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims);

    string GenerateRefreshToken();

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
