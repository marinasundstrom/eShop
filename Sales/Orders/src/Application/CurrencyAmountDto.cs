using System.Linq.Expressions;
using System.Reflection;

namespace YourBrand.Orders.Application;

public record CurrencyAmountDto(string Currency, decimal Amount);