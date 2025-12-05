namespace _05_DependencyInversionPrinciple.BadExample;

public class OrderService
{
    // High-level module depends directly on low-level modules
    private readonly SqlDatabase _database = new();
    private readonly EmailSender _emailSender = new();

    public void PlaceOrder(string product, int quantity)
    {
        _database.Save($"Order: {product} x {quantity}");
        _emailSender.Send($"Order placed: {product}");
    }
}

public class SqlDatabase
{
    public List<string> SavedItems { get; } = [];
    public void Save(string data) => SavedItems.Add(data);
}

public class EmailSender
{
    public List<string> SentEmails { get; } = [];
    public void Send(string message) => SentEmails.Add(message);
}

