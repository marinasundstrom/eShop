using FluentValidation;
using MediatR;

namespace YourBrand.Payments.Application.Features.Receipts.Commands;

public sealed record DeleteReceipt(string Id) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<DeleteReceipt>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<DeleteReceipt, Result>
    {
        private readonly IReceiptRepository orderRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(IReceiptRepository orderRepository, IUnitOfWork unitOfWork)
        {
            this.orderRepository = orderRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteReceipt request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FindByIdAsync(request.Id, cancellationToken);

            if (order is null)
            {
                return Result.Failure(Errors.Receipts.ReceiptNotFound);
            }

            orderRepository.Remove(order);

            order.AddDomainEvent(new ReceiptDeleted(order.ReceiptNo));

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
