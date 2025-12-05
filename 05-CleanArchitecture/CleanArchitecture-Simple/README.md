# Clean Architecture

> *"The center of your application is not the database. Nor is it one or more of the frameworks you may be using. The center of your application is the use cases of your application."*
> — Robert C. Martin, *Clean Architecture: A Craftsman's Guide to Software Structure and Design* (2017)

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

## Related Architectures

Clean, Onion, and Hexagonal Architecture share the same core idea: **protect the domain from infrastructure concerns**.

| Concept | Clean Architecture | Onion Architecture | Hexagonal (Ports & Adapters) |
|---------|-------------------|-------------------|------------------------------|
| **Author** | Robert C. Martin (2012) | Jeffrey Palermo (2008) | Alistair Cockburn (2005) |
| **Core** | Domain / Entities | Domain Model | Domain |
| **Business Logic** | Use Cases | Domain Services | Application |
| **Abstractions** | Interfaces (in Application) | Interfaces (in Core) | Ports |
| **External Systems** | Infrastructure | Infrastructure | Adapters |
| **Entry Points** | Presentation / Controllers | UI / API | Primary Adapters |
| **Outgoing** | Repositories, Gateways | Repositories | Secondary Adapters |

### Key Differences

| Aspect | Clean | Onion | Hexagonal |
|--------|-------|-------|-----------|
| **Focus** | Separation by policy level | Layer dependencies | Symmetry of I/O |
| **Layers** | 4 explicit circles | Concentric rings | Inside/Outside only |
| **Terminology** | Use Cases, Entities | Domain Services, Domain Model | Ports, Adapters |
| **Visualization** | Concentric circles | Onion rings | Hexagon with ports |

All three enforce: **dependencies point inward, domain has no external dependencies**.

---
