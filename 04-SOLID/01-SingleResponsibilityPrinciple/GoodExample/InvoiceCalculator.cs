namespace _01_SingleResponsibilityPrinciple.GoodExample;

/// <summary>
/// ✅ Single Responsibility: Calculate invoice amounts
/// Only changes when calculation logic changes (tax rules, discounts, etc.)
/// </summary>
public class InvoiceCalculator
{
    public decimal CalculateSubtotal(Invoice invoice)
    {
        return invoice.Items.Sum(item => item.Quantity * item.UnitPrice);
    }

    public decimal CalculateTax(Invoice invoice)
    {
        return CalculateSubtotal(invoice) * invoice.TaxRate;
    }

    public decimal CalculateTotal(Invoice invoice)
    {
        return CalculateSubtotal(invoice) + CalculateTax(invoice);
    }
}

