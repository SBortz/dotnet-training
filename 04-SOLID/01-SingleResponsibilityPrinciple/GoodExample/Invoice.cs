namespace _01_SingleResponsibilityPrinciple.GoodExample;

/// <summary>
/// ✅ Follows SRP - Invoice is just a data container
/// </summary>
public record Invoice(
    string CustomerName,
    List<InvoiceItem> Items,
    decimal TaxRate = 0.19m
);

public record InvoiceItem(string Description, int Quantity, decimal UnitPrice);

