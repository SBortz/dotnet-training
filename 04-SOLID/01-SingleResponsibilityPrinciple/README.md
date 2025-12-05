# Single Responsibility Principle (SRP)

> *"A class should have only one reason to change."*
> — Robert C. Martin

Later clarified:

> *"Gather together the things that change for the same reasons. Separate those things that change for different reasons."*

---

## The Invoice Example

### Bad: One class does everything

```csharp
public class Invoice
{
    public decimal CalculateTotal() { ... }  // Reason 1: Calculation logic
    public string ExportToPdf() { ... }      // Reason 2: PDF format
    public string Print() { ... }            // Reason 3: Printer API
}
```

Tax calculation changes? → Modify `Invoice`.  
PDF layout changes? → Modify `Invoice`.  
Printer API changes? → Modify `Invoice`.

### Good: Separated responsibilities

```csharp
public record Invoice(string CustomerName, List<InvoiceItem> Items, decimal TaxRate);

public class InvoiceCalculator  { decimal CalculateTotal(Invoice i) { ... } }
public class InvoicePdfExporter { string Export(Invoice i) { ... } }
public class InvoicePrinter     { string Print(Invoice i) { ... } }
```

Each class has one reason to change. Changes are isolated.

---

## Identifying SRP Violations

Ask yourself:
1. **"What does this class do?"** — If "and" appears in the answer, SRP suspect
2. **"Who might request changes?"** — Different stakeholders = different responsibilities
3. **"Can I test this in isolation?"** — If you need unrelated dependencies, consider splitting

---

## Typical Violations

| Violation | Solution |
|-----------|----------|
| Entity saves itself | Separate `Entity` + `Repository` |
| Service validates and processes | Separate `Validator` + `Processor` |
| Controller handles logic and formatting | Separate `Controller`, `Service`, `Formatter` |

---
