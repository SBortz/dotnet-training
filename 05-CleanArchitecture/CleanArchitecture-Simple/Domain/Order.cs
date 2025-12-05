namespace CleanArchitecture_Simple.Domain;

// Innermost layer - no dependencies on other layers
public class Order
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Product { get; }
    public int Quantity { get; }
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;

    public Order(string product, int quantity)
    {
        if (string.IsNullOrWhiteSpace(product))
            throw new ArgumentException("Product required", nameof(product));
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));
        
        Product = product;
        Quantity = quantity;
    }

    public void Confirm() => Status = OrderStatus.Confirmed;
    public void Ship() => Status = OrderStatus.Shipped;
}

public enum OrderStatus { Pending, Confirmed, Shipped }

