# Liskov Substitution Principle (LSP)

> *"Let φ(x) be a property provable about objects x of type T. Then φ(y) should be true for objects y of type S, where S is a subtype of T."*
> — Barbara Liskov

Simpler: If `S` is a subtype of `T`, you must be able to replace `T` with `S` without changing program behavior.

---

## Signature Rules

| Rule | Description |
|------|-------------|
| **Contravariance** | Parameters in subtypes must not be more specific |
| **Covariance** | Return types in subtypes may be more specific |
| **Exceptions** | Subtypes must not throw new exception types |

---

## Contravariance (Parameters)

An overridden method must not expect a more specific type as parameter.

```csharp
IContravariant<Car> carSetter = new CarSetter();
IContravariant<SportsCar> sportsCarSetter = carSetter;  // Contravariance
```

## Covariance (Return Types)

An overridden method may return a more specific type.

```csharp
ICovariant<SportsCar> sportsCarGetter = new SportsCarGetter();
ICovariant<Car> carGetter = sportsCarGetter;  // Covariance
```

---

## Generic Variance in C# (`in` / `out`)

Without `in`/`out`, generic types are **invariant** — `IFoo<Car>` and `IFoo<SportsCar>` are completely different types.

### `in` = Contravariance (input only)

```csharp
public interface IContravariant<in T>
{
    void Set(T value);  // allowed
    // T Get();         // Compiler error!
}
```

Enables: `IContravariant<Car>` → `IContravariant<SportsCar>`

### `out` = Covariance (output only)

```csharp
public interface ICovariant<out T>
{
    T Get();           // allowed
    // void Set(T x);  // Compiler error!
}
```

Enables: `ICovariant<SportsCar>` → `ICovariant<Car>`

---

## .NET Examples

| Interface | Variance | Reason |
|-----------|----------|--------|
| `IEnumerable<out T>` | Covariant | Only produces T |
| `IComparer<in T>` | Contravariant | Only consumes T |
| `Func<in T, out TResult>` | Both | T is input, TResult is output |

---

## Variance Problems in Other Languages

### Java: Unsafe Array Covariance

```java
Object[] objects = new String[3];  // Compiles
objects[0] = 42;                   // Runtime: ArrayStoreException!
```

### TypeScript: Bivariant Function Parameters

```typescript
let animalHandler: (a: Animal) => void;
let dogHandler: (d: Dog) => void = (d) => console.log(d.breed);

animalHandler = dogHandler;  // Allowed, but unsafe!
animalHandler({ name: "Cat" });  // Runtime: 'breed' is undefined
```

C# has the same array problem (historical), but generics are safe via `in`/`out`.

---
