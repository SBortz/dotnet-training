# Interface Segregation Principle (ISP)

> *"Clients should not be forced to depend on interfaces they do not use."*
> — Robert C. Martin

---

## Das Problem: Fat Interfaces

### Bad: IWorker zwingt Robot zu unsinnigen Methoden

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

Robot muss `Eat()` und `Sleep()` implementieren, obwohl er das nicht kann.

---

## Die Lösung: Kleine, spezifische Interfaces

### Good: Jedes Gerät implementiert nur was es braucht

```csharp
public interface IPrinter { void Print(string doc); }
public interface IScanner { string Scan(); }
public interface IFax { void Fax(string doc, string number); }

public class SimplePrinter : IPrinter { ... }
public class MultiFunctionDevice : IPrinter, IScanner, IFax { ... }
```

`SimplePrinter` muss weder `IScanner` noch `IFax` implementieren.

---

## Typische Verstöße

| Verstoß | Lösung |
|---------|--------|
| `IRepository` mit 20 Methoden | Aufteilen in `IReadRepository`, `IWriteRepository` |
| `IUserService` macht Auth + Profile + Settings | Separate Services pro Concern |
| God-Interface mit allem | Role Interfaces nach Verwendungszweck |

---

