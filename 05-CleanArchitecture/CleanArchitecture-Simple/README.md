# Clean Architecture

> *"The center of your application is not the database. Nor is it one or more of the frameworks you may be using. The center of your application is the use cases of your application."*
> — Robert C. Martin

---

## The Dependency Rule

Dependencies point **inward only**. Inner layers know nothing about outer layers.

```
┌─────────────────────────────────────────────┐
│  Presentation (Controllers, UI)             │
│  ┌─────────────────────────────────────┐    │
│  │  Infrastructure (DB, Email, APIs)   │    │
│  │  ┌─────────────────────────────┐    │    │
│  │  │  Application (Use Cases)    │    │    │
│  │  │  ┌─────────────────────┐    │    │    │
│  │  │  │  Domain (Entities)  │    │    │    │
│  │  │  └─────────────────────┘    │    │    │
│  │  └─────────────────────────────┘    │    │
│  └─────────────────────────────────────┘    │
└─────────────────────────────────────────────┘
          ←── Dependencies point inward
```

---

## Layers

| Layer | Contains | Depends on |
|-------|----------|------------|
| **Domain** | Entities, Value Objects, Business Rules | Nothing |
| **Application** | Use Cases, Ports (interfaces) | Domain |
| **Infrastructure** | Repositories, External Services | Application, Domain |
| **Presentation** | Controllers, DTOs | Application |

---

## Example Structure

```
Domain/
  └── Order.cs              # Entity with business logic

Application/
  ├── Ports.cs              # IOrderRepository, INotificationService
  └── PlaceOrderUseCase.cs  # Orchestrates domain + ports

Infrastructure/
  └── SqlOrderRepository.cs # Implements IOrderRepository

Presentation/
  └── OrderController.cs    # HTTP → UseCase → Response
```

---

## Key Insight: Dependency Inversion

Application defines **interfaces** (ports).  
Infrastructure **implements** them.

```csharp
// Application layer defines what it needs
public interface IOrderRepository { void Save(Order order); }

// Infrastructure layer provides implementation
public class SqlOrderRepository : IOrderRepository { ... }
```

This allows swapping SQL for Mongo, Email for SMS, etc. without touching Application or Domain.

---

