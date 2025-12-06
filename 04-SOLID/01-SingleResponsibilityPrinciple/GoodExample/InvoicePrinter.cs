namespace _01_SingleResponsibilityPrinciple.GoodExample;

/// <summary>
/// ✅ Single Responsibility: Print invoice
/// Only changes when printing requirements change
/// </summary>
public class InvoicePrinter
{
    private readonly InvoiceCalculator _calculator;

    public InvoicePrinter(InvoiceCalculator calculator)
    {
        _calculator = calculator;
    }

    public string Print(Invoice invoice)
    {
        var total = _calculator.CalculateTotal(invoice);
        var itemLines = invoice.Items
            .Select(i => $"{i.Description}: {i.Quantity} x {i.UnitPrice:C}");

        return $"""
            ================================
            INVOICE - {invoice.CustomerName}
            --------------------------------
            {string.Join("\n", itemLines)}
            --------------------------------
            Total: {total:C}
            ================================
            """;
    }
}


