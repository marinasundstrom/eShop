namespace Site.Client.Authentication;

public interface IAuthenticationService
{
    Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication);
    Task Logout();
    Task<string> RefreshToken();
}

public class UserForAuthenticationDto
{
    public string SSN { get; set; }
}

public class AuthResponseDto
{
    public string RefreshToken { get; set; }
    public string Token { get; set; }
    public bool IsAuthSuccessful { get; set; }
}