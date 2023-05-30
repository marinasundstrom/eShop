using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.Payments.Application.Features.Receipts.Dtos;

namespace YourBrand.Payments.Application.Features.Receipts.Commands;

public sealed record CreateReceipt(string? CustomerId, BillingDetailsDto BillingDetails, ShippingDetailsDto? ShippingDetails, IEnumerable<CreateReceiptItemDto> Items) : IRequest<Result<ReceiptDto>>
{
    public sealed class Validator : AbstractValidator<CreateReceipt>
    {
        public Validator()
        {
            //RuleFor(x => x.Title).NotEmpty().MaximumLength(60);

            //RuleFor(x => x.Description).MaximumLength(240);
        }
    }

    public sealed class Handler : IRequestHandler<CreateReceipt, Result<ReceiptDto>>
    {
        private readonly IReceiptRepository orderRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDomainEventDispatcher domainEventDispatcher;

        public Handler(IReceiptRepository orderRepository, IUnitOfWork unitOfWork, IDomainEventDispatcher domainEventDispatcher)
        {
            this.orderRepository = orderRepository;
            this.unitOfWork = unitOfWork;
            this.domainEventDispatcher = domainEventDispatcher;
        }

        public async Task<Result<ReceiptDto>> Handle(CreateReceipt request, CancellationToken cancellationToken)
        {
            var order = new Receipt();
            order.ReceiptNo = (await orderRepository.GetAll().MaxAsync(x => x.ReceiptNo)) + 1;
            order.StatusId = 3;

            order.CustomerId = request.CustomerId;

            order.VatIncluded = true;

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
                order.AddReceiptItem(orderItem.Description, orderItem.ItemId, orderItem.Unit, orderItem.UnitPrice, orderItem.VatRate, orderItem.Quantity, orderItem.Notes);
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

            await domainEventDispatcher.Dispatch(new ReceiptCreated(order.Id), cancellationToken);

            order = await orderRepository.GetAll()
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .FirstAsync(x => x.ReceiptNo == order.ReceiptNo, cancellationToken);

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
