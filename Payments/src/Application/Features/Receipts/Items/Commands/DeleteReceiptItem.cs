using FluentValidation;
using MediatR;

namespace YourBrand.Payments.Application.Features.Receipts.Items.Commands;

public sealed record DeleteReceiptItem(string ReceiptId, string OrdeItemId) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<DeleteReceiptItem>
    {
        public Validator()
        {
            RuleFor(x => x.ReceiptId).NotEmpty();

            RuleFor(x => x.OrdeItemId).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<DeleteReceiptItem, Result>
    {
        private readonly IReceiptRepository orderRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(IReceiptRepository orderRepository, IUnitOfWork unitOfWork)
        {
            this.orderRepository = orderRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteReceiptItem request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FindByIdAsync(request.ReceiptId, cancellationToken);

            if (order is null)
            {
                return Result.Failure(Errors.Receipts.ReceiptNotFound);
            }

            var orderItem = order.Items.FirstOrDefault(x => x.Id == request.OrdeItemId);

            if (orderItem is null)
            {
                throw new System.Exception();
            }

            order.RemoveReceiptItem(orderItem);

            order.Calculate();

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
