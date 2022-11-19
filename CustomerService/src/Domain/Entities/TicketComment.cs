using YourBrand.CustomerService.Domain.Enums;
using YourBrand.CustomerService.Domain.Events;

namespace YourBrand.CustomerService.Domain.Entities;

public class TicketComment : Entity<int>, IAuditable
{
    public string Text { get; set; } = null!;

    public HashSet<Attachment> Attachments { get; } = new HashSet<Attachment>();

    // ...

    public User? CreatedBy { get; set; } = null!;

    public string? CreatedById { get; set; } = null!;

    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}