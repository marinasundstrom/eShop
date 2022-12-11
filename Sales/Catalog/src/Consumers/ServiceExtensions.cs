using MassTransit;

namespace YourBrand.Catalog.Consumers;

public static class ServiceExtensions
{
    public static IBusRegistrationConfigurator AddConsumersForApp(this IBusRegistrationConfigurator busRegistrationConfigurator)
    {
        busRegistrationConfigurator.AddConsumer<QuantityAvailableChangedConsumer>();

        return busRegistrationConfigurator;
    }
}
