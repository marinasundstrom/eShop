using FluentValidation;
using MediatR;
using YourBrand.Payments.Application.Features.Receipts.Dtos;

namespace YourBrand.Payments.Application.Features.Receipts.Queries;

public record GetReceiptByNo(int ReceiptNo) : IRequest<Result<ReceiptDto>>
{
    public class Validator : AbstractValidator<GetReceiptById>
    {
        public Validator()
        {
            RuleFor(x => x.Id);
        }
    }

    public class Handler : IRequestHandler<GetReceiptByNo, Result<ReceiptDto>>
    {
        private readonly IReceiptRepository orderRepository;

        public Handler(IReceiptRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<Result<ReceiptDto>> Handle(GetReceiptByNo request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FindByNoAsync(request.ReceiptNo, cancellationToken);

            if (order is null)
            {
                return Result.Failure<ReceiptDto>(Errors.Receipts.ReceiptNotFound);
            }

            return Result.Success(order.ToDto());
        }
    }
}