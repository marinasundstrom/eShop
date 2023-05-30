using FluentValidation;
using MediatR;
using YourBrand.Payments.Application.Features.Receipts.Dtos;

namespace YourBrand.Payments.Application.Features.Receipts.Queries;

public record GetReceiptById(string Id) : IRequest<Result<ReceiptDto>>
{
    public class Validator : AbstractValidator<GetReceiptById>
    {
        public Validator()
        {
            RuleFor(x => x.Id);
        }
    }

    public class Handler : IRequestHandler<GetReceiptById, Result<ReceiptDto>>
    {
        private readonly IReceiptRepository orderRepository;

        public Handler(IReceiptRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<Result<ReceiptDto>> Handle(GetReceiptById request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FindByIdAsync(request.Id, cancellationToken);

            if (order is null)
            {
                return Result.Failure<ReceiptDto>(Errors.Receipts.ReceiptNotFound);
            }

            return Result.Success(order.ToDto());
        }
    }
}