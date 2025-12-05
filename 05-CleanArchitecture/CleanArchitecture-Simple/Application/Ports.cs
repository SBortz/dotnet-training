namespace CleanArchitecture_Simple.Application;

using CleanArchitecture_Simple.Domain;

// Ports = interfaces defined by Application layer
// Infrastructure implements these (Dependency Inversion)

public interface IOrderRepository
{
    void Save(Order order);
    Order? GetById(Guid id);
}

public interface INotificationService
{
    void SendOrderConfirmation(Order order);
}

