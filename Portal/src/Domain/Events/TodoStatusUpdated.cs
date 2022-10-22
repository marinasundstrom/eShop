using YourBrand.Portal.Domain.Enums;

namespace YourBrand.Portal.Domain.Events;

public sealed record TodoStatusUpdated(int TodoId, TodoStatus NewStatus, TodoStatus OldStatus) : DomainEvent;