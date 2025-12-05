namespace CleanArchitecture_Simple.Infrastructure;

using CleanArchitecture_Simple.Application;
using CleanArchitecture_Simple.Domain;

// Infrastructure implements Application ports
// Depends on: Application (for interfaces), Domain (for entities)

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly Dictionary<Guid, Order> _orders = [];

    public void Save(Order order) => _orders[order.Id] = order;
    public Order? GetById(Guid id) => _orders.GetValueOrDefault(id);
    
    // Test helper
    public IReadOnlyCollection<Order> GetAll() => _orders.Values.ToList();
}

public class EmailNotificationService : INotificationService
{
    public List<string> SentNotifications { get; } = [];

    public void SendOrderConfirmation(Order order)
    {
        SentNotifications.Add($"Order {order.Id}: {order.Product} x {order.Quantity}");
    }
}

