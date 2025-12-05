# Liskov Substitution Principle (LSP)

> *"Let φ(x) be a property provable about objects x of type T. Then φ(y) should be true for objects y of type S, where S is a subtype of T."*
> — Barbara Liskov

Einfacher: Wenn `S` ein Subtyp von `T` ist, muss man `T` durch `S` ersetzen können, ohne das Programmverhalten zu ändern.

---

## Signatur-Regeln

| Regel | Beschreibung |
|-------|--------------|
| **Kontravarianz** | Parameter in Subtypen dürfen nicht spezifischer sein |
| **Kovarianz** | Rückgabetypen in Subtypen dürfen spezifischer sein |
| **Exceptions** | Subtypen dürfen keine neuen Exception-Typen werfen |

---

## Kontravarianz (Parameter)

Eine überschriebene Methode darf keinen spezifischeren Typ als Parameter erwarten.

```csharp
IContravariant<Car> carSetter = new CarSetter();
IContravariant<SportsCar> sportsCarSetter = carSetter;  // Kontravarianz
```

## Kovarianz (Rückgabetypen)

Eine überschriebene Methode darf einen spezifischeren Typ zurückgeben.

```csharp
ICovariant<SportsCar> sportsCarGetter = new SportsCarGetter();
ICovariant<Car> carGetter = sportsCarGetter;  // Kovarianz
```

---

## Generic Variance in C# (`in` / `out`)

Ohne `in`/`out` sind generische Typen **invariant** — `IFoo<Car>` und `IFoo<SportsCar>` sind komplett verschiedene Typen.

### `in` = Kontravarianz (nur Input)

```csharp
public interface IContravariant<in T>
{
    void Set(T value);  // erlaubt
    // T Get();         // Compiler-Fehler!
}
```

Ermöglicht: `IContravariant<Car>` → `IContravariant<SportsCar>`

### `out` = Kovarianz (nur Output)

```csharp
public interface ICovariant<out T>
{
    T Get();           // erlaubt
    // void Set(T x);  // Compiler-Fehler!
}
```

Ermöglicht: `ICovariant<SportsCar>` → `ICovariant<Car>`

---

## .NET Beispiele

| Interface | Varianz | Grund |
|-----------|---------|-------|
| `IEnumerable<out T>` | Kovariant | Produziert nur T |
| `IComparer<in T>` | Kontravariant | Konsumiert nur T |
| `Func<in T, out TResult>` | Beides | T ist Input, TResult ist Output |

---

## Varianz-Probleme in anderen Sprachen

### Java: Unsichere Array-Kovarianz

```java
Object[] objects = new String[3];  // Kompiliert
objects[0] = 42;                   // Runtime: ArrayStoreException!
```

### TypeScript: Bivariante Funktionsparameter

```typescript
let animalHandler: (a: Animal) => void;
let dogHandler: (d: Dog) => void = (d) => console.log(d.breed);

animalHandler = dogHandler;  // Erlaubt, aber unsicher!
animalHandler({ name: "Cat" });  // Runtime: 'breed' is undefined
```

C# hat das gleiche Array-Problem (historisch), aber Generics sind sicher durch `in`/`out`.

---
