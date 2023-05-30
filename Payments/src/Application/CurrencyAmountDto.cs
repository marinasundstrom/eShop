using System.Linq.Expressions;
using System.Reflection;

namespace YourBrand.Payments.Application;

public record CurrencyAmountDto(string Currency, decimal Amount);