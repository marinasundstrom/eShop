namespace YourBrand.CustomerService.Domain.Entities;

public class TicketStatus : Entity<int>
{
    public string Name { get; set; } = null!;
}