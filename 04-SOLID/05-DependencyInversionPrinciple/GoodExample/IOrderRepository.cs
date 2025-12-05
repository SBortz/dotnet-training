namespace _05_DependencyInversionPrinciple.GoodExample;

public interface IOrderRepository
{
    void Save(string order);
}

public interface INotificationService
{
    void Notify(string message);
}

