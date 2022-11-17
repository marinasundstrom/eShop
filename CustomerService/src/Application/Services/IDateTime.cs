namespace YourBrand.CustomerService.Application.Services;

public interface IDateTime
{
    DateTimeOffset Now { get; }
}

