namespace YourBrand.StoreFront.Authentication;

public class User
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public string Email { get; set; }

    public List<UserRefreshToken> RefreshTokens { get; set; } = new List<UserRefreshToken>();

}

public class UserRefreshToken
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }
}

