using YourBrand.CustomerService.Domain.Enums;
using YourBrand.CustomerService.Domain.Events;

namespace YourBrand.CustomerService.Domain.Entities;

public class Ticket : AggregateRoot<int>, IAuditable
{
    public Ticket(string title, string? description, TicketStatus status = TicketStatus.NotStarted)
        : base(0)
    {
        Title = title;
        Description = description;
        Status = status;
    }

    public string Title { get; private set; } = null!;

    public bool UpdateTitle(string title)
    {
        var oldTitle = Title;
        if (title != oldTitle)
        {
            Title = title;

            AddDomainEvent(new TicketUpdated(Id));
            AddDomainEvent(new TicketTitleUpdated(Id, title));

            return true;
        }

        return false;
    }

    public string? Description { get; private set; }

    public bool UpdateDescription(string? description)
    {
        var oldDescription = Description;
        if (description != oldDescription)
        {
            Description = description;

            AddDomainEvent(new TicketUpdated(Id));
            AddDomainEvent(new TicketDescriptionUpdated(Id, description));

            return true;
        }

        return false;
    }

    public TicketStatus Status { get; private set; }

    public bool UpdateStatus(TicketStatus status)
    {
        var oldStatus = Status;
        if (status != oldStatus)
        {
            Status = status;

            AddDomainEvent(new TicketUpdated(Id));
            AddDomainEvent(new TicketStatusUpdated(Id, status, oldStatus));

            return true;
        }

        return false;
    }

    public User? Assignee { get; private set; }

    public string? AssigneeId { get; private set; }

    public bool UpdateAssigneeId(string? userId)
    {
        var oldAssigneeId = AssigneeId;
        if (userId != oldAssigneeId)
        {
            AssigneeId = userId;
            AddDomainEvent(new TicketAssignedUserUpdated(Id, userId, oldAssigneeId));

            return true;
        }

        return false;
    }

    public double? EstimatedHours { get; private set; }

    public bool UpdateEstimatedHours(double? hours)
    {
        var oldHours = EstimatedHours;
        if (hours != oldHours)
        {
            EstimatedHours = hours;

            AddDomainEvent(new TicketUpdated(Id));
            AddDomainEvent(new TicketEstimatedHoursUpdated(Id, hours, oldHours));

            return true;
        }

        return false;
    }

    public double? RemainingHours { get; private set; }

    public bool UpdateRemainingHours(double? hours)
    {
        var oldHours = RemainingHours;
        if (hours != oldHours)
        {
            RemainingHours = hours;

            AddDomainEvent(new TicketUpdated(Id));
            AddDomainEvent(new TicketRemainingHoursUpdated(Id, hours, oldHours));

            return true;
        }

        return false;
    }

    public User ?CreatedBy { get; set; } = null!;

    public string? CreatedById { get; set; } = null!;

    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}
