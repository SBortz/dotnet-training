# Interface Segregation Principle (ISP)

> *"Clients should not be forced to depend on interfaces they do not use."*
> — Robert C. Martin

---

## The Problem: Fat Interfaces

### Bad: IWorker forces Robot to implement useless methods

```csharp
public interface IWorker
{
    void Work();
    void Eat();
    void Sleep();
}

public class Robot : IWorker
{
    public void Work() { /* ok */ }
    public void Eat() => throw new NotSupportedException();  // !
    public void Sleep() => throw new NotSupportedException(); // !
}
```

Robot must implement `Eat()` and `Sleep()` even though it can't do either.

---

## The Solution: Small, specific interfaces

### Good: Each device implements only what it needs

```csharp
public interface IPrinter { void Print(string doc); }
public interface IScanner { string Scan(); }
public interface IFax { void Fax(string doc, string number); }

public class SimplePrinter : IPrinter { ... }
public class MultiFunctionDevice : IPrinter, IScanner, IFax { ... }
```

`SimplePrinter` doesn't need to implement `IScanner` or `IFax`.

---

## Typical Violations

| Violation | Solution |
|-----------|----------|
| `IRepository` with 20 methods | Split into `IReadRepository`, `IWriteRepository` |
| `IUserService` does Auth + Profile + Settings | Separate services per concern |
| God-interface with everything | Role interfaces by use case |

---
