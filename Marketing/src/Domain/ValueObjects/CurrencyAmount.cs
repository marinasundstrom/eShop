namespace YourBrand.Marketing.Domain.ValueObjects;

public sealed class CurrencyAmount : ValueObject
{
    internal CurrencyAmount()
    {

    }

    public CurrencyAmount(string currency, decimal amount)
    {
        Currency = currency;
        Amount = amount;
    }

    public string Currency { get; private set; } = null!;

    public decimal Amount { get; private set; }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Currency;
        yield return Amount;
    }
}