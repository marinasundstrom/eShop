using MassTransit;

namespace YourBrand.Sales.Consumers;

public static class ServiceExtensions
{
    public static IBusRegistrationConfigurator AddConsumersForApp(this IBusRegistrationConfigurator busRegistrationConfigurator)
    {
        busRegistrationConfigurator.AddConsumer<UpdateStatusConsumer>();

        return busRegistrationConfigurator;
    }
}
