using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.Payments.Application.Features.Receipts.Dtos;

namespace YourBrand.Payments.Application.Features.Receipts.Items.Commands;

public sealed record CreateReceiptItem(string ReceiptId, string Description, string? ItemId, string? Unit, decimal UnitPrice, double VatRate, double Quantity, string? Notes) : IRequest<Result<ReceiptItemDto>>
{
    public sealed class Validator : AbstractValidator<CreateReceiptItem>
    {
        public Validator()
        {
            RuleFor(x => x.ReceiptId);

            RuleFor(x => x.Description).NotEmpty().MaximumLength(240);
        }
    }

    public sealed class Handler : IRequestHandler<CreateReceiptItem, Result<ReceiptItemDto>>
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

        public async Task<Result<ReceiptItemDto>> Handle(CreateReceiptItem request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FindByIdAsync(request.ReceiptId, cancellationToken);

            if (order is null)
            {
                return Result.Failure<ReceiptItemDto>(Errors.Receipts.ReceiptNotFound);
            }

            var orderItem = order.AddReceiptItem(request.Description, request.ItemId, request.Unit, request.UnitPrice, request.VatRate, request.Quantity, request.Notes);

            order.Calculate();

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(orderItem!.ToDto());
        }
    }
}
