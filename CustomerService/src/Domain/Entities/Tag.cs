namespace YourBrand.CustomerService.Domain.Entities;

public class Tag : Entity<int>
{
    public string Name { get; set; } = null!;
}
