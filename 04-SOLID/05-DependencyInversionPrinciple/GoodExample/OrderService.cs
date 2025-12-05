namespace _05_DependencyInversionPrinciple.GoodExample;

public class OrderService(IOrderRepository repository, INotificationService notification)
{
    public void PlaceOrder(string product, int quantity)
    {
        repository.Save($"Order: {product} x {quantity}");
        notification.Notify($"Order placed: {product}");
    }
}

