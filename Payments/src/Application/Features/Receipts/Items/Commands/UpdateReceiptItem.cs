using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.Payments.Application.Features.Receipts.Dtos;

namespace YourBrand.Payments.Application.Features.Receipts.Items.Commands;

public sealed record UpdateReceiptItem(string ReceiptId, string ReceiptItemId, string Description, string? ItemId, string? Unit, decimal UnitPrice, double VatRate, double Quantity, string? Notes) : IRequest<Result<ReceiptItemDto>>
{
    public sealed class Validator : AbstractValidator<UpdateReceiptItem>
    {
        public Validator()
        {
            RuleFor(x => x.ReceiptId);

            RuleFor(x => x.Description).NotEmpty().MaximumLength(240);
        }
    }

    public sealed class Handler : IRequestHandler<UpdateReceiptItem, Result<ReceiptItemDto>>
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

        public async Task<Result<ReceiptItemDto>> Handle(UpdateReceiptItem request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FindByIdAsync(request.ReceiptId, cancellationToken);

            if (order is null)
            {
                return Result.Failure<ReceiptItemDto>(Errors.Receipts.ReceiptNotFound);
            }

            var orderItem = order.Items.FirstOrDefault(x => x.Id == request.ReceiptItemId);

            if (orderItem is null)
            {
                throw new System.Exception();
            }

            orderItem.Description = request.Description;
            orderItem.ItemId = request.ItemId;
            orderItem.Unit = request.Unit;
            orderItem.UnitPrice = request.UnitPrice;
            orderItem.VatRate = request.VatRate;
            orderItem.Quantity = request.Quantity;
            orderItem.Notes = request.Notes;

            order.Calculate();

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(orderItem!.ToDto());
        }
    }
}
