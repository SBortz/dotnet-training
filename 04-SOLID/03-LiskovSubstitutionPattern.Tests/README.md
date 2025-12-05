# Liskov Substitution Principle (LSP)

> *"Let φ(x) be a property provable about objects x of type T. Then φ(y) should be true for objects y of type S, where S is a subtype of T."*

The LSP was formulated by **Barbara Liskov** in the late 1980s and further developed in the 1990s together with **Jeannette Wing**. It is closely related to *Design by Contract* by Bertrand Meyer.

## Summary

**In subtypes: Add new behaviors and state – don't change existing ones.**

In simpler terms: If `S` is a subtype of `T`, we can replace objects of type `T` with objects of type `S` without changing the expected behavior of the program (correctness).

---

## Signature Requirements of the LSP

| Rule | Description |
|------|-------------|
| **Contravariance** | Parameters of methods in subtypes must be contravariant |
| **Covariance** | Return types of methods in subtypes must be covariant |
| **Exceptions** | Subtypes must not throw new exception types |

> In C#, breaking the first two rules is challenging – unless you intentionally try to.

## Contravariance (Parameters)

- An overridden method must **not expect a more specific subtype** in its parameters than the base type.
- LSP adds: **Preconditions** cannot be stricter in subtypes.
- **Reason**: The subtype could not be substituted for the base type otherwise.

```csharp
// Example: CarSetter accepts Car (base type)
// Can also be used as IContravariant<SportsCar>
IContravariant<Car> carSetter = new CarSetter();
IContravariant<SportsCar> sportsCarSetter = carSetter;  // ✓ Contravariance
```

## Covariance (Return Types)

- When a subtype overrides a method, it may return a **more specific subtype**.
- LSP adds: **Postconditions** can be stricter.
- **Reason**: A more precise return type does not break the substitution principle.

```csharp
// Example: SportsCarGetter returns SportsCar (subtype of Car)
// Can also be used as ICovariant<Car>
ICovariant<SportsCar> sportsCarGetter = new SportsCarGetter();
ICovariant<Car> carGetter = sportsCarGetter;  // ✓ Covariance
```

---

## Additional Behavior Rules

- ✅ Subtypes must **pass all tests** written for the supertype
- ✅ Subtypes may only **add state and functionality**
- ❌ Subtypes must **not modify the supertype's state**

## Exceptions

Throwing new exception types in subtypes is considered a behavior change.

**Allowed:** Throwing subtypes of existing exceptions – these can be handled by existing consumers.



---

## Generic Variance in C# (`in` / `out`)

C# enforces variance through the `in` and `out` keywords on generic type parameters. Without these keywords, generic types are **invariant** – meaning `IFoo<Car>` and `IFoo<SportsCar>` are completely unrelated types.

`in` / `out` make generic types assignment-compatible:

| Without Variance | With `in` (Contravariance) | With `out` (Covariance) |
|------------------|----------------------------|-------------------------|
| `IFoo<Car>` ≠ `IFoo<SportsCar>` | `IFoo<Car>` → `IFoo<SportsCar>` ✓ | `IFoo<SportsCar>` → `IFoo<Car>` ✓ |


### `in` = Contravariance (input only)

```csharp
public interface IContravariant<in T>  // T can only be accepted, not returned
{
    void Set(T value);  // ✓ allowed
    // T Get();         // ✗ compile error!
}
```

Enables: `IContravariant<Car>` → `IContravariant<SportsCar>`
The actual implementation does not get stricter – it still accepts any `Car`. The interface variable can be typed more specifically, but the instance remains safely substitutable.

### `out` = Covariance (output only)

```csharp
public interface ICovariant<out T>  // T can only be returned, not accepted
{
    T Get();           // ✓ allowed
    // void Set(T x);  // ✗ compile error!
}
```

Enables: `ICovariant<SportsCar>` → `ICovariant<Car>`
The implementation returns a more specific type (`SportsCar`). Since every `SportsCar` is a `Car`, consumers expecting a `Car` will always receive a valid value.

### Examples from the .NET Framework

| Interface | Variance | Reason |
|-----------|----------|--------|
| `IEnumerable<out T>` | Covariant | Only produces T |
| `IComparer<in T>` | Contravariant | Only consumes T |
| `Func<in T, out TResult>` | Both | T is input, TResult is output |

---

## Variance Problems in Other Languages

C#'s `in`/`out` keywords enforce type safety at **compile time**. Other languages handle variance differently, sometimes leading to runtime errors.

### Java: Unsafe Array Covariance

Java arrays are covariant, which can cause **runtime exceptions**:

```java
Object[] objects = new String[3];  // ✓ Compiles (arrays are covariant)
objects[0] = 42;                   // ✗ Runtime: ArrayStoreException!
```

The compiler cannot catch this error. C# has the same problem with arrays (for historical reasons), but generics are safe.

### TypeScript: Bivariant Function Parameters (by design)

TypeScript treats method parameters as **bivariant** (both co- and contravariant) for practicality:

```typescript
interface Animal { name: string }
interface Dog extends Animal { breed: string }

let animalHandler: (a: Animal) => void;
let dogHandler: (d: Dog) => void = (d) => console.log(d.breed);

animalHandler = dogHandler;  // ✓ Allowed, but unsafe!
animalHandler({ name: "Cat" });  // Runtime: 'breed' is undefined
```
