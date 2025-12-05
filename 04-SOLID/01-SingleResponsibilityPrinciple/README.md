# Single Responsibility Principle (SRP)

> *"A module should be responsible to one, and only one, actor."*

The term **actor** refers to a group (consisting of one or more stakeholders or users) that requires a change in the module.

**Robert C. Martin**, the originator of the term, expresses the principle as:

> *"A class should have only one reason to change."*

Because of confusion around the word "reason", he later clarified his meaning:

> *"Gather together the things that change for the same reasons. Separate those things that change for different reasons."*

The principle is also about **roles or actors**. For example, while they might be the same person, the role of an accountant is different from a database administrator. Hence, each module should be responsible for each role.

---

## Summary

**Each class should do one thing and do it well.**

When a class has multiple responsibilities, changes to one responsibility may break the others. This leads to:
- Tight coupling
- Difficult testing
- Fragile code

---

## The Invoice Example

### ❌ Bad Example: One Class Does Everything

```csharp
public class Invoice
{
    public decimal CalculateTotal() { ... }  // Reason 1: Calculation logic
    public string ExportToPdf() { ... }      // Reason 2: PDF format
    public string Print() { ... }            // Reason 3: Printing
}
```

**Problems:**
- If tax calculation changes → modify `Invoice`
- If PDF layout changes → modify `Invoice`
- If printer API changes → modify `Invoice`
- Testing calculation requires the whole class (including PDF/print dependencies)

### ✅ Good Example: Separated Responsibilities

```csharp
public record Invoice(string CustomerName, List<InvoiceItem> Items, decimal TaxRate);

public class InvoiceCalculator    { decimal CalculateTotal(Invoice i) { ... } }
public class InvoicePdfExporter   { string Export(Invoice i) { ... } }
public class InvoicePrinter       { string Print(Invoice i) { ... } }
```

**Benefits:**
- Each class has **one reason to change**
- Classes can be **tested independently**
- Changes are **isolated** – PDF changes don't affect calculation
- Easy to **mock dependencies** in tests

---

## How to Identify SRP Violations

Ask yourself:
1. **"What does this class do?"** – If you use "and" in the answer, it might violate SRP
2. **"Who might request changes?"** – Different stakeholders = different responsibilities
3. **"Can I test this in isolation?"** – If you need unrelated dependencies, consider splitting

### Example Answers:

❌ *"This class calculates totals **and** exports PDFs **and** prints invoices."*

✅ *"This class calculates invoice totals."*

---

## Common SRP Violations

| Violation | Solution |
|-----------|----------|
| Entity saves itself to database | Separate `Entity` and `Repository` |
| Service validates and processes | Separate `Validator` and `Processor` |
| Controller handles logic and formatting | Separate `Controller`, `Service`, `Formatter` |
| Class logs its own actions | Inject an `ILogger` dependency |

---
