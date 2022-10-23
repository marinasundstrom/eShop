using System;
using YourBrand.Orders.Domain.Specifications;

namespace YourBrand.Orders.Infrastructure.Persistence.Repositories.Mocks;

public sealed class MockOrderRepository : IOrderRepository
{
    private readonly MockUnitOfWork mockUnitOfWork;

    public MockOrderRepository(MockUnitOfWork mockUnitOfWork)
    {
        this.mockUnitOfWork = mockUnitOfWork;
    }

    public void Add(Order item)
    {
        mockUnitOfWork.Items.Add(item);
        mockUnitOfWork.NewItems.Add(item);
    }

    public void Dispose()
    {
        foreach (var item in mockUnitOfWork.NewItems)
        {
            mockUnitOfWork.Items.Remove(item);
        }
    }

    public Task<Order?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = mockUnitOfWork.Items
            .OfType<Order>()
            .FirstOrDefault(x => x.Id.Equals(id));

        return Task.FromResult(item);
    }

    public IQueryable<Order> GetAll()
    {
        return mockUnitOfWork.Items
            .OfType<Order>()
            .AsQueryable();
    }

    public IQueryable<Order> GetAll(ISpecification<Order> specification)
    {
        return mockUnitOfWork.Items
            .OfType<Order>()
            .AsQueryable()
            .Where(specification.Criteria);
    }

    public void Remove(Order item)
    {
        mockUnitOfWork.Items.Remove(item);
    }
}

