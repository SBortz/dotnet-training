# Single Responsibility Principle (SRP)

> *"A class should have only one reason to change."*
> — Robert C. Martin

Später präzisiert:

> *"Gather together the things that change for the same reasons. Separate those things that change for different reasons."*

---

## Das Invoice-Beispiel

### Bad: Eine Klasse macht alles

```csharp
public class Invoice
{
    public decimal CalculateTotal() { ... }  // Änderungsgrund 1: Berechnungslogik
    public string ExportToPdf() { ... }      // Änderungsgrund 2: PDF-Format
    public string Print() { ... }            // Änderungsgrund 3: Drucker-API
}
```

Steuerberechnung ändert sich? → `Invoice` ändern.  
PDF-Layout ändert sich? → `Invoice` ändern.  
Drucker-API ändert sich? → `Invoice` ändern.

### Good: Getrennte Verantwortlichkeiten

```csharp
public record Invoice(string CustomerName, List<InvoiceItem> Items, decimal TaxRate);

public class InvoiceCalculator  { decimal CalculateTotal(Invoice i) { ... } }
public class InvoicePdfExporter { string Export(Invoice i) { ... } }
public class InvoicePrinter     { string Print(Invoice i) { ... } }
```

Jede Klasse hat einen Änderungsgrund. Änderungen sind isoliert.

---

## SRP-Verstöße erkennen

Frag dich:
1. **"Was macht diese Klasse?"** — Wenn "und" in der Antwort vorkommt, SRP-Verdacht
2. **"Wer könnte Änderungen verlangen?"** — Verschiedene Stakeholder = verschiedene Responsibilities
3. **"Kann ich das isoliert testen?"** — Wenn du unrelated Dependencies brauchst, aufteilen

---

## Typische Verstöße

| Verstoß | Lösung |
|---------|--------|
| Entity speichert sich selbst | `Entity` + `Repository` trennen |
| Service validiert und verarbeitet | `Validator` + `Processor` trennen |
| Controller macht Logik und Formatierung | `Controller`, `Service`, `Formatter` trennen |

---
