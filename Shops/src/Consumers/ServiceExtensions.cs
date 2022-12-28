using MassTransit;

namespace YourBrand.Shops.Consumers;

public static class ServiceExtensions
{
    public static IBusRegistrationConfigurator AddConsumersForApp(this IBusRegistrationConfigurator busRegistrationConfigurator)
    {
        busRegistrationConfigurator.AddConsumer<UpdateStatusConsumer>();

        return busRegistrationConfigurator;
    }
}
