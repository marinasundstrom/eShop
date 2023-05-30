using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.Payments.Application.Features.Receipts.Dtos;

namespace YourBrand.Payments.Application.Features.Receipts.Commands;

public sealed record CreateDraftReceipt() : IRequest<Result<ReceiptDto>>
{
    public sealed class Validator : AbstractValidator<CreateDraftReceipt>
    {
        public Validator()
        {
            //RuleFor(x => x.Title).NotEmpty().MaximumLength(60);

            //RuleFor(x => x.Description).MaximumLength(240);
        }
    }

    public sealed class Handler : IRequestHandler<CreateDraftReceipt, Result<ReceiptDto>>
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

        public async Task<Result<ReceiptDto>> Handle(CreateDraftReceipt request, CancellationToken cancellationToken)
        {
            var order = new Receipt();
            order.ReceiptNo = (await orderRepository.GetAll().MaxAsync(x => x.ReceiptNo)) + 1;

            order.VatIncluded = true;

            orderRepository.Add(order);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await domainEventDispatcher.Dispatch(new ReceiptCreated(order.Id), cancellationToken);

            order = await orderRepository.GetAll()
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .FirstAsync(x => x.ReceiptNo == order.ReceiptNo, cancellationToken);

            return Result.Success(order!.ToDto());
        }
    }
}
