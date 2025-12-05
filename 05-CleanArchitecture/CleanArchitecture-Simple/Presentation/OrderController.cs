namespace CleanArchitecture_Simple.Presentation;

using CleanArchitecture_Simple.Application;

// Presentation layer - thin, delegates to Use Cases
// Depends on: Application (use cases)
// Does NOT directly depend on: Infrastructure

public class OrderController(PlaceOrderUseCase placeOrder)
{
    public OrderResponse PlaceOrder(OrderRequest request)
    {
        var order = placeOrder.Execute(request.Product, request.Quantity);
        
        return new OrderResponse(order.Id, order.Status.ToString());
    }
}

public record OrderRequest(string Product, int Quantity);
public record OrderResponse(Guid OrderId, string Status);

