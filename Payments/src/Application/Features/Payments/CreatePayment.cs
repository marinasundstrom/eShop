using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Quickpay.Models.Payments;
using Quickpay.RequestParams;
using Quickpay.Services;

namespace YourBrand.Payments.Application.Features.Payments;

public sealed record CreatePayment() : IRequest<Result<string>>
{
    public sealed class Validator : AbstractValidator<CreatePayment>
    {
        public Validator()
        {
            //RuleFor(x => x.Title).NotEmpty().MaximumLength(60);

            //RuleFor(x => x.Description).MaximumLength(240);
        }
    }

    public sealed class Handler : IRequestHandler<CreatePayment, Result<string>>
    {
        private readonly IConfiguration configuration;
        private readonly PaymentsService paymentService;
        private readonly IDomainEventDispatcher domainEventDispatcher;

        public Handler(IConfiguration configuration, PaymentsService paymentService, IDomainEventDispatcher domainEventDispatcher)
        {
            this.configuration = configuration;
            this.paymentService = paymentService;
            this.domainEventDispatcher = domainEventDispatcher;
        }

        public async Task<Result<string>> Handle(CreatePayment request, CancellationToken cancellationToken)
        {
            var createPaymentParams = new CreatePaymentRequestParams("SEK", createRandomOrderId());
            createPaymentParams.text_on_statement = "QuickPay .NET Example";

            var basketItemJeans = new Basket();
            basketItemJeans.qty = 1;
            basketItemJeans.item_name = "Jeans";
            basketItemJeans.item_price = 100;
            basketItemJeans.vat_rate = 0.25;
            basketItemJeans.item_no = "123";

            var basketItemShirt = new Basket();
            basketItemShirt.qty = 1;
            basketItemShirt.item_name = "Shirt";
            basketItemShirt.item_price = 300;
            basketItemShirt.vat_rate = 0.25;
            basketItemShirt.item_no = "321";

            createPaymentParams.basket = new Basket[] { basketItemJeans, basketItemShirt };

            var payment = await paymentService.CreatePayment(createPaymentParams);

            // Second we must create a payment link for the payment. This payment link can be opened in a browser to show the payment window from QuickPay.
            var createPaymentLinkParams = new CreatePaymentLinkRequestParams((int)((basketItemJeans.qty * basketItemJeans.item_price + basketItemShirt.qty * basketItemShirt.item_price) * 100));
            //createPaymentLinkParams.payment_methods = "creditcard";
            createPaymentLinkParams.auto_capture = true; // This will automatically capture the payment right after it has been authorized.
            createPaymentLinkParams.callback_url = configuration["QuickPay:CallbackUrl"];
            var paymentLink = await paymentService.CreateOrUpdatePaymentLink(payment.id, createPaymentLinkParams);

            return Result.Success(paymentLink!.url!.ToString()!);
        }

        static string createRandomOrderId()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
        }
    }
}
