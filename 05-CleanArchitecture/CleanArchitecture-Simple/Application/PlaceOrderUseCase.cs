namespace CleanArchitecture_Simple.Application;

using CleanArchitecture_Simple.Domain;

// Use Case - orchestrates domain objects and ports
// Depends on: Domain, Ports (interfaces)
// Does NOT depend on: Infrastructure, Presentation

public class PlaceOrderUseCase(IOrderRepository repository, INotificationService notification)
{
    public Order Execute(string product, int quantity)
    {
        var order = new Order(product, quantity);
        order.Confirm();
        
        repository.Save(order);
        notification.SendOrderConfirmation(order);
        
        return order;
    }
}

