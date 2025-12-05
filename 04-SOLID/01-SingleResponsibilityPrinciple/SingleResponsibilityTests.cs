namespace _01_SingleResponsibilityPrinciple;

using Bad = _01_SingleResponsibilityPrinciple.BadExample;
using Good = _01_SingleResponsibilityPrinciple.GoodExample;

public class SingleResponsibilityTests
{
    #region Bad Example Tests
    
    [Test]
    public void BadExample_Invoice_CalculatesTotal()
    {
        // The Invoice class does everything - calculation, export, print
        var invoice = new Bad.Invoice
        {
            CustomerName = "Acme Corp",
            Items = [
                new Bad.InvoiceItem("Widget", 2, 10.00m),
                new Bad.InvoiceItem("Gadget", 1, 25.00m)
            ],
            TaxRate = 0.19m
        };

        // Testing calculation - but this class also handles PDF and printing!
        Assert.That(invoice.CalculateSubtotal(), Is.EqualTo(45.00m));
        Assert.That(invoice.CalculateTax(), Is.EqualTo(8.55m));
        Assert.That(invoice.CalculateTotal(), Is.EqualTo(53.55m));
    }

    [Test]
    public void BadExample_Invoice_ExportAndPrint_AreTightlyCoupled()
    {
        var invoice = new Bad.Invoice
        {
            CustomerName = "Test Customer",
            Items = [new Bad.InvoiceItem("Item", 1, 100.00m)],
            TaxRate = 0.10m
        };

        // Problem: If PDF format changes, we modify the same class as calculation
        // Problem: If printing requirements change, we modify the same class
        var pdf = invoice.ExportToPdf();
        var printed = invoice.Print();

        Assert.That(pdf, Does.Contain("[PDF DOCUMENT]"));
        Assert.That(printed, Does.Contain("INVOICE"));
    }

    #endregion

    #region Good Example Tests
    
    [Test]
    public void GoodExample_Calculator_OnlyCalculates()
    {
        // Calculator has ONE responsibility: calculations
        var invoice = new Good.Invoice(
            "Acme Corp",
            [
                new Good.InvoiceItem("Widget", 2, 10.00m),
                new Good.InvoiceItem("Gadget", 1, 25.00m)
            ],
            0.19m
        );

        var calculator = new Good.InvoiceCalculator();

        Assert.That(calculator.CalculateSubtotal(invoice), Is.EqualTo(45.00m));
        Assert.That(calculator.CalculateTax(invoice), Is.EqualTo(8.55m));
        Assert.That(calculator.CalculateTotal(invoice), Is.EqualTo(53.55m));
    }

    [Test]
    public void GoodExample_PdfExporter_OnlyExports()
    {
        // PDF Exporter has ONE responsibility: PDF generation
        var invoice = new Good.Invoice(
            "Test Customer",
            [new Good.InvoiceItem("Item", 1, 100.00m)],
            0.10m
        );

        var calculator = new Good.InvoiceCalculator();
        var pdfExporter = new Good.InvoicePdfExporter(calculator);

        var pdf = pdfExporter.Export(invoice);

        Assert.That(pdf, Does.Contain("[PDF DOCUMENT]"));
        Assert.That(pdf, Does.Contain("Test Customer"));
    }

    [Test]
    public void GoodExample_Printer_OnlyPrints()
    {
        // Printer has ONE responsibility: printing
        var invoice = new Good.Invoice(
            "Test Customer",
            [new Good.InvoiceItem("Item", 1, 100.00m)],
            0.10m
        );

        var calculator = new Good.InvoiceCalculator();
        var printer = new Good.InvoicePrinter(calculator);

        var printed = printer.Print(invoice);

        Assert.That(printed, Does.Contain("INVOICE"));
        Assert.That(printed, Does.Contain("Test Customer"));
    }

    [Test]
    public void GoodExample_Components_CanBeTestedIndependently()
    {
        // Key benefit: Each component can be tested in isolation
        // If calculation logic changes, only InvoiceCalculator tests need updating
        // If PDF format changes, only InvoicePdfExporter tests need updating
        
        var invoice = new Good.Invoice(
            "Independent Test",
            [new Good.InvoiceItem("Test", 1, 50.00m)],
            0.20m
        );

        // We can test calculator without caring about PDF or printing
        var calculator = new Good.InvoiceCalculator();
        Assert.That(calculator.CalculateTotal(invoice), Is.EqualTo(60.00m));
    }

    #endregion
}

