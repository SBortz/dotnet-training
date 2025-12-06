namespace _01_SingleResponsibilityPrinciple.BadExample;

/// <summary>
/// ❌ Violates SRP - This class has THREE reasons to change:
/// 1. Calculation logic changes (tax rates, discounts)
/// 2. PDF export format changes (layout, styling)
/// 3. Printing requirements change (printer API, formatting)
/// </summary>
public class Invoice
{
    public string CustomerName { get; set; } = "";
    public List<InvoiceItem> Items { get; set; } = [];
    public decimal TaxRate { get; set; } = 0.19m;

    // Reason 1: Calculation logic
    public decimal CalculateSubtotal()
    {
        return Items.Sum(item => item.Quantity * item.UnitPrice);
    }

    public decimal CalculateTax()
    {
        return CalculateSubtotal() * TaxRate;
    }

    public decimal CalculateTotal()
    {
        return CalculateSubtotal() + CalculateTax();
    }

    // Reason 2: PDF export format
    public string ExportToPdf()
    {
        // Simulated PDF generation
        return $"""
            [PDF DOCUMENT]
            Invoice for: {CustomerName}
            Items: {Items.Count}
            Subtotal: {CalculateSubtotal():C}
            Tax ({TaxRate:P0}): {CalculateTax():C}
            Total: {CalculateTotal():C}
            [END PDF]
            """;
    }

    // Reason 3: Printing
    public string Print()
    {
        // Simulated printing
        return $"""
            ================================
            INVOICE - {CustomerName}
            --------------------------------
            {string.Join("\n", Items.Select(i => $"{i.Description}: {i.Quantity} x {i.UnitPrice:C}"))}
            --------------------------------
            Total: {CalculateTotal():C}
            ================================
            """;
    }
}

public record InvoiceItem(string Description, int Quantity, decimal UnitPrice);


