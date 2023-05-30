using FluentValidation;
using MediatR;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Quickpay.Models.Payments;
using Quickpay.RequestParams;
using Quickpay.Services;

namespace YourBrand.Payments.Application.Features.Payments;

public sealed record QuickPayCallback(CallbackData Data) : IRequest
{
    public sealed class Validator : AbstractValidator<QuickPayCallback>
    {
        public Validator()
        {
            //RuleFor(x => x.Title).NotEmpty().MaximumLength(60);

            //RuleFor(x => x.Description).MaximumLength(240);
        }
    }

    public sealed class Handler : IRequestHandler<QuickPayCallback>
    {
        public async Task<Unit> Handle(QuickPayCallback request, CancellationToken cancellationToken)
        {
            return Unit.Value;
        }
    }
}

public partial class CallbackData
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("ulid")]
    public string Ulid { get; set; } = default!;

    [JsonPropertyName("merchant_id")]
    public long MerchantId { get; set; }

    [JsonPropertyName("order_id")]
    public string OrderId { get; set; } = default!;

    [JsonPropertyName("accepted")]
    public bool Accepted { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = default!;

    [JsonPropertyName("text_on_statement")]
    public string TextOnStatement { get; set; } = default!;

    [JsonPropertyName("branding_id")]
    public string? BrandingId { get; set; }

    [JsonPropertyName("variables")]
    public Variables Variables { get; set; } = default!;

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = default!;

    [JsonPropertyName("state")]
    public string State { get; set; } = default!;

    [JsonPropertyName("metadata")]
    public Metadata Metadata { get; set; } = default!;

    [JsonPropertyName("link")]
    public Link Link { get; set; } = default!;

    [JsonPropertyName("shipping_address")]
    public string? ShippingAddress { get; set; }

    [JsonPropertyName("invoice_address")]
    public string? InvoiceAddress { get; set; }

    [JsonPropertyName("basket")]
    public Basket2[] Basket { get; set; } = default!;

    [JsonPropertyName("shipping")]
    public string? Shipping { get; set; }

    [JsonPropertyName("operations")]
    public Operation[] Operations { get; set; } = default!;

    [JsonPropertyName("test_mode")]
    public bool TestMode { get; set; }

    [JsonPropertyName("acquirer")]
    public string Acquirer { get; set; } = default!;

    [JsonPropertyName("facilitator")]
    public string? Facilitator { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonPropertyName("retented_at")]
    public DateTime? RetentedAt { get; set; }

    [JsonPropertyName("balance")]
    public long Balance { get; set; }

    [JsonPropertyName("fee")]
    public decimal? Fee { get; set; }

    [JsonPropertyName("deadline_at")]
    public DateTime? DeadlineAt { get; set; }

    [JsonPropertyName("reseller_id")]
    public long ResellerId { get; set; }
}

public partial class Basket2
{
    [JsonPropertyName("qty")]
    public long Qty { get; set; }

    [JsonPropertyName("item_no")]
    public string ItemNo { get; set; } = default!;

    [JsonPropertyName("item_name")]
    public string ItemName { get; set; } = default!;

    [JsonPropertyName("item_price")]
    public long ItemPrice { get; set; }

    [JsonPropertyName("vat_rate")]
    public double VatRate { get; set; }
}

public partial class Link
{
    [JsonPropertyName("url")]
    public Uri Url { get; set; } = default!;

    [JsonPropertyName("agreement_id")]
    public long AgreementId { get; set; }

    [JsonPropertyName("language")]
    public string Language { get; set; } = default!;

    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    [JsonPropertyName("continue_url")]
    public string? ContinueUrl { get; set; }

    [JsonPropertyName("cancel_url")]
    public string? CancelUrl { get; set; }

    [JsonPropertyName("callback_url")]
    public Uri CallbackUrl { get; set; } = default!;

    [JsonPropertyName("payment_methods")]
    public string? PaymentMethods { get; set; }

    [JsonPropertyName("auto_fee")]
    public bool AutoFee { get; set; }

    [JsonPropertyName("auto_capture")]
    public bool AutoCapture { get; set; }

    [JsonPropertyName("auto_capture_at")]
    public DateTimeOffset AutoCaptureAt { get; set; }

    [JsonPropertyName("branding_id")]
    public string? BrandingId { get; set; }

    [JsonPropertyName("google_analytics_client_id")]
    public string? GoogleAnalyticsClientId { get; set; }

    [JsonPropertyName("google_analytics_tracking_id")]
    public string? GoogleAnalyticsTrackingId { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; } = default!;

    [JsonPropertyName("acquirer")]
    public string? Acquirer { get; set; }

    [JsonPropertyName("deadline")]
    public DateTime? Deadline { get; set; }

    [JsonPropertyName("framed")]
    public bool Framed { get; set; }

    [JsonPropertyName("branding_config")]
    public Variables BrandingConfig { get; set; } = default!;

    [JsonPropertyName("invoice_address_selection")]
    public string? InvoiceAddressSelection { get; set; }

    [JsonPropertyName("shipping_address_selection")]
    public string? ShippingAddressSelection { get; set; }

    [JsonPropertyName("fee_vat")]
    public decimal? FeeVat { get; set; }

    [JsonPropertyName("moto")]
    public bool Moto { get; set; }

    [JsonPropertyName("customer_email")]
    public string? CustomerEmail { get; set; }
}

public partial class Variables
{
}

public partial class Metadata
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = default!;

    [JsonPropertyName("origin")]
    public string Origin { get; set; } = default!;

    [JsonPropertyName("brand")]
    public string Brand { get; set; } = default!;

    [JsonPropertyName("bin")]
    public string Bin { get; set; } = default!;

    [JsonPropertyName("corporate")]
    public bool Corporate { get; set; }

    [JsonPropertyName("last4")]
    public string Last4 { get; set; } = default!;

    [JsonPropertyName("exp_month")]
    public long ExpMonth { get; set; }

    [JsonPropertyName("exp_year")]
    public long ExpYear { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; } = default!;

    [JsonPropertyName("is_3d_secure")]
    public bool Is3DSecure { get; set; }

    [JsonPropertyName("3d_secure_type")]
    public string The3DSecureType { get; set; } = default!;

    [JsonPropertyName("issued_to")]
    public string IssuedTo { get; set; } = default!;

    [JsonPropertyName("hash")]
    public string Hash { get; set; } = default!;

    [JsonPropertyName("moto")]
    public bool Moto { get; set; }

    [JsonPropertyName("number")]
    public long? Number { get; set; }

    [JsonPropertyName("customer_ip")]
    public string CustomerIp { get; set; } = default!;

    [JsonPropertyName("customer_country")]
    public string CustomerCountry { get; set; } = default!;

    [JsonPropertyName("fraud_suspected")]
    public bool FraudSuspected { get; set; }

    [JsonPropertyName("fraud_remarks")]
    public string[] FraudRemarks { get; set; } = default!;

    [JsonPropertyName("fraud_reported")]
    public bool FraudReported { get; set; }

    [JsonPropertyName("fraud_report_description")]
    public string? FraudReportDescription { get; set; }

    [JsonPropertyName("fraud_reported_at")]
    public DateTime? FraudReportedAt { get; set; }

    [JsonPropertyName("nin_number")]
    public string? NinNumber { get; set; }

    [JsonPropertyName("nin_country_code")]
    public string? NinCountryCode { get; set; }

    [JsonPropertyName("nin_gender")]
    public string? NinGender { get; set; }

    [JsonPropertyName("shopsystem_name")]
    public string? ShopsystemName { get; set; }

    [JsonPropertyName("shopsystem_version")]
    public string? ShopsystemVersion { get; set; }
}

public partial class Operation
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = default!;

    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    [JsonPropertyName("pending")]
    public bool Pending { get; set; }

    [JsonPropertyName("qp_status_code")]
    public string QpStatusCode { get; set; } = default!;

    [JsonPropertyName("qp_status_msg")]
    public string QpStatusMsg { get; set; } = default!;

    [JsonPropertyName("aq_status_code")]
    public string? AqStatusCode { get; set; }

    [JsonPropertyName("aq_status_msg")]
    public string AqStatusMsg { get; set; } = default!;

    [JsonPropertyName("data")]
    public IDictionary<string, object> Data { get; set; } = default!;

    [JsonPropertyName("callback_url")]
    public Uri CallbackUrl { get; set; } = default!;

    [JsonPropertyName("callback_success")]
    public bool? CallbackSuccess { get; set; }

    [JsonPropertyName("callback_response_code")]
    public string? CallbackResponseCode { get; set; }

    [JsonPropertyName("callback_duration")]
    public int? CallbackDuration { get; set; }

    [JsonPropertyName("acquirer")]
    public string Acquirer { get; set; } = default!;

    [JsonPropertyName("3d_secure_status")]
    public string The3DSecureStatus { get; set; } = default!;

    [JsonPropertyName("callback_at")]
    public DateTime? CallbackAt { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}
