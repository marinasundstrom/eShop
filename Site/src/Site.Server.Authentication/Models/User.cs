namespace Site.Server.Authentication;

public class User
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public string Email { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }
}

