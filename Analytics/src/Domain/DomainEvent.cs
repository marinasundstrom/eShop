﻿using MediatR;

namespace YourBrand.Analytics.Domain
{
    public abstract record DomainEvent : INotification
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}