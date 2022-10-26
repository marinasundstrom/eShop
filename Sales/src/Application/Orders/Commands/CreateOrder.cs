using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.Sales.Application.Orders.Dtos;

namespace YourBrand.Sales.Application.Orders.Commands;

public sealed record CreateOrder(string? CustomerId, BillingDetailsDto BillingDetails, ShippingDetailsDto? ShippingDetails, IEnumerable<CreateOrderItemDto> Items) : IRequest<Result<OrderDto>>
{
    public sealed class Validator : AbstractValidator<CreateOrder>
    {
        public Validator()
        {
            //RuleFor(x => x.Title).NotEmpty().MaximumLength(60);

            //RuleFor(x => x.Description).MaximumLength(240);
        }
    }

    public sealed class Handler : IRequestHandler<CreateOrder, Result<OrderDto>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDomainEventDispatcher domainEventDispatcher;

        public Handler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IDomainEventDispatcher domainEventDispatcher)
        {
            this.orderRepository = orderRepository;
            this.unitOfWork = unitOfWork;
            this.domainEventDispatcher = domainEventDispatcher;
        }

        public async Task<Result<OrderDto>> Handle(CreateOrder request, CancellationToken cancellationToken)
        {
            var order = new Order();

            order.CustomerId = request.CustomerId;

            order.BillingDetails = new Domain.ValueObjects.BillingDetails
            {
                FirstName = request.BillingDetails.FirstName,
                LastName = request.BillingDetails.LastName,
                SSN = request.BillingDetails.SSN,
                Email = request.BillingDetails.Email,
                PhoneNumber = request.BillingDetails.PhoneNumber,
                Address = Map(request.BillingDetails.Address)
            };

            if (request.ShippingDetails is not null)
            {
                order.ShippingDetails = new Domain.ValueObjects.ShippingDetails
                {
                    FirstName = request.ShippingDetails.FirstName,
                    LastName = request.ShippingDetails.LastName,
                    CareOf = request.ShippingDetails.CareOf,
                    Address = Map(request.ShippingDetails.Address),
                };
            }

            foreach (var orderItem in request.Items)
            {
                order.AddOrderItem(orderItem.Description, orderItem.ItemId, orderItem.Unit, orderItem.UnitPrice, orderItem.VatRate, orderItem.Quantity);
            }

            order.Calculate();

            orderRepository.Add(order);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            /*
            if (request.AssigneeId is not null)
            {
                order.UpdateAssigneeId(request.AssigneeId);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                order.ClearDomainEvents();
            }
            */

            await domainEventDispatcher.Dispatch(new OrderCreated(order.Id), cancellationToken);

            order = await orderRepository.GetAll()
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .FirstAsync(x => x.Id == order.Id, cancellationToken);

            return Result.Success(order!.ToDto());
        }

        private Domain.ValueObjects.Address Map(AddressDto address)
        {
            return new Domain.ValueObjects.Address
            {
                Thoroughfare = address.Thoroughfare,
                Premises = address.Premises,
                SubPremises = address.SubPremises,
                PostalCode = address.PostalCode,
                Locality = address.Locality,
                SubAdministrativeArea = address.SubAdministrativeArea,
                AdministrativeArea = address.AdministrativeArea,
                Country = address.Country
            };
        }
    }
}
