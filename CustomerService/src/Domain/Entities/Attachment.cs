namespace YourBrand.CustomerService.Domain.Entities;

public class Attachment : Entity<int>
{
    public string Name { get; set; } = null!;
}
