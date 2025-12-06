namespace _01_SingleResponsibilityPrinciple.GoodExample;

/// <summary>
/// ✅ Single Responsibility: Export invoice to PDF format
/// Only changes when PDF format requirements change
/// </summary>
public class InvoicePdfExporter
{
    private readonly InvoiceCalculator _calculator;

    public InvoicePdfExporter(InvoiceCalculator calculator)
    {
        _calculator = calculator;
    }

    public string Export(Invoice invoice)
    {
        var subtotal = _calculator.CalculateSubtotal(invoice);
        var tax = _calculator.CalculateTax(invoice);
        var total = _calculator.CalculateTotal(invoice);

        return $"""
            [PDF DOCUMENT]
            Invoice for: {invoice.CustomerName}
            Items: {invoice.Items.Count}
            Subtotal: {subtotal:C}
            Tax ({invoice.TaxRate:P0}): {tax:C}
            Total: {total:C}
            [END PDF]
            """;
    }
}


