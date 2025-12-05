namespace CleanArchitecture_Simple;

using CleanArchitecture_Simple.Application;
using CleanArchitecture_Simple.Domain;
using CleanArchitecture_Simple.Infrastructure;
using CleanArchitecture_Simple.Presentation;

public class CleanArchitectureTests
{
    [Test]
    public void Domain_HasNoDependencies_OnOtherLayers()
    {
        // Domain layer only knows about itself
        var order = new Order("Widget", 5);
        
        order.Confirm();
        
        Assert.That(order.Status, Is.EqualTo(OrderStatus.Confirmed));
        Assert.That(order.Product, Is.EqualTo("Widget"));
    }

    [Test]
    public void Application_DependsOnlyOn_DomainAndPorts()
    {
        // Application uses interfaces (ports), not concrete implementations
        var repository = new InMemoryOrderRepository();
        var notification = new EmailNotificationService();
        var useCase = new PlaceOrderUseCase(repository, notification);
        
        var order = useCase.Execute("Gadget", 3);
        
        Assert.That(order.Status, Is.EqualTo(OrderStatus.Confirmed));
        Assert.That(repository.GetAll(), Has.Count.EqualTo(1));
        Assert.That(notification.SentNotifications, Has.Count.EqualTo(1));
    }

    [Test]
    public void Presentation_DelegatesToUseCases()
    {
        // Controller is thin - just transforms request/response
        var repository = new InMemoryOrderRepository();
        var notification = new EmailNotificationService();
        var useCase = new PlaceOrderUseCase(repository, notification);
        var controller = new OrderController(useCase);
        
        var response = controller.PlaceOrder(new OrderRequest("Widget", 2));
        
        Assert.That(response.Status, Is.EqualTo("Confirmed"));
        Assert.That(response.OrderId, Is.Not.EqualTo(Guid.Empty));
    }
}

