namespace _05_DependencyInversionPrinciple.GoodExample;

public class SqlOrderRepository : IOrderRepository
{
    public List<string> SavedOrders { get; } = [];
    public void Save(string order) => SavedOrders.Add(order);
}

public class EmailNotificationService : INotificationService
{
    public List<string> SentNotifications { get; } = [];
    public void Notify(string message) => SentNotifications.Add(message);
}

// Easy to add alternatives without changing OrderService
public class SmsNotificationService : INotificationService
{
    public List<string> SentSms { get; } = [];
    public void Notify(string message) => SentSms.Add($"SMS: {message}");
}

