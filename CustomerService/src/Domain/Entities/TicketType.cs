namespace YourBrand.CustomerService.Domain.Entities;

public class TicketType : Entity<int>
{
    public string Name { get; set; } = null!;
}
