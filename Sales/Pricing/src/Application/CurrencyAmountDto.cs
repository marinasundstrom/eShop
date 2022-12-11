using System.Linq.Expressions;
using System.Reflection;

namespace YourBrand.Pricing.Application;

public record CurrencyAmountDto(string Currency, decimal Amount);