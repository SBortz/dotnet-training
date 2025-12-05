namespace _05_DependencyInversionPrinciple;

using Bad = _05_DependencyInversionPrinciple.BadExample;
using Good = _05_DependencyInversionPrinciple.GoodExample;

public class DependencyInversionTests
{
    [Test]
    public void BadExample_OrderService_TightlyCoupledToImplementations()
    {
        // Can't substitute dependencies - they're hardcoded
        var service = new Bad.OrderService();
        
        service.PlaceOrder("Widget", 2);
        
        // Works, but:
        // - Can't test without real SqlDatabase/EmailSender behavior
        // - Can't swap Email for SMS without modifying OrderService
        // - Can't use different database without modifying OrderService
        Assert.Pass("OrderService works but is tightly coupled");
    }

    [Test]
    public void GoodExample_OrderService_DependsOnAbstractions()
    {
        var repository = new Good.SqlOrderRepository();
        var notification = new Good.EmailNotificationService();
        var service = new Good.OrderService(repository, notification);
        
        service.PlaceOrder("Widget", 2);
        
        Assert.That(repository.SavedOrders, Has.Count.EqualTo(1));
        Assert.That(notification.SentNotifications, Has.Count.EqualTo(1));
        
        // Can easily swap implementations
        var smsNotification = new Good.SmsNotificationService();
        var serviceWithSms = new Good.OrderService(repository, smsNotification);
        
        serviceWithSms.PlaceOrder("Gadget", 1);
        
        Assert.That(smsNotification.SentSms, Has.Count.EqualTo(1));
        Assert.That(smsNotification.SentSms[0], Does.Contain("SMS:"));
    }
}

