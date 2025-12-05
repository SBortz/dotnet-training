# Dependency Inversion Principle (DIP)

> *"High-level modules should not depend on low-level modules. Both should depend on abstractions."*
> — Robert C. Martin

---

## The Problem: Direct dependencies

### Bad: OrderService creates its dependencies directly

```csharp
public class OrderService
{
    private readonly SqlDatabase _database = new();
    private readonly EmailSender _emailSender = new();

    public void PlaceOrder(string product, int quantity)
    {
        _database.Save(...);
        _emailSender.Send(...);
    }
}
```

- Can't test without real DB/Email
- Swap Email for SMS? → Modify OrderService
- Different database? → Modify OrderService

---

## The Solution: Depend on abstractions

### Good: Dependencies are injected

```csharp
public interface IOrderRepository { void Save(string order); }
public interface INotificationService { void Notify(string message); }

public class OrderService(IOrderRepository repo, INotificationService notification)
{
    public void PlaceOrder(string product, int quantity)
    {
        repo.Save(...);
        notification.Notify(...);
    }
}
```

- Easily testable with mocks
- Email/SMS/Push swappable without changing OrderService
- SQL/Mongo/InMemory swappable

---

## Typical Violations

| Violation | Solution |
|-----------|----------|
| `new ConcreteService()` in constructor | Interface + Dependency Injection |
| Static helper classes | Interface + inject instance |
| Direct file/database access | Repository Pattern |

---
