# 🔍 C# Equality & Identity Training

This project demonstrates how **equality (`==`, `.Equals()`)** and **identity (`ReferenceEquals`)** behave in C#. It covers reference types, value types, structs, operator overloading, boxing, and interning.

The code is structured into clear examples, each with an explanation and output.

---

## 🎯 Purpose

This training covers:

- ✅ Value vs reference equality
- ✅ Overriding `==` and `.Equals()`
- ✅ Boxing and its effects on comparison
- ✅ Interning with `string`
- ✅ `ReferenceEquals` for strict identity checking
- ✅ Best practices for domain-driven design (DDD) value objects

---

# 🧪 List of Examples – Equality & Identity in C#

This is a list of all examples included in `Program.cs`, with a short explanation for each.

| #    | Name                                                  | Description                                                                 |
|------|-------------------------------------------------------|-----------------------------------------------------------------------------|
| 1    | `==` with reference types (not overridden)            | Compares two objects of the same class with identical content → `false` due to reference comparison. |
| 2    | `==` with reference types (overridden for value)      | Uses overloaded `==` in `PersonC` class to compare values semantically.     |
| 3    | `==` with built-in value types (`int`)                | Compares values directly → returns `true` for equal values.                 |
| 4    | `==` with `string`                                    | `string` overrides `==` to compare content, not reference.                 |
| 5    | `==` with custom value types (struct)                 | Not allowed unless explicitly overloaded → compile error otherwise.        |

---

| #    | Name                                                  | Description                                                                 |
|------|-------------------------------------------------------|-----------------------------------------------------------------------------|
| 6    | `.Equals()` with reference types (not overridden)     | Default `Equals()` compares references → returns `false` for same content. |
| 7    | `.Equals()` with reference types (overridden)         | `PersonB` overrides `Equals()` to enable semantic value comparison.        |
| 8    | `.Equals()` with built-in value types (`int`)         | Built-in types override `Equals()` to compare values directly.             |
| 9    | `.Equals()` with custom struct                        | Structs support default field-wise comparison via `Equals()` → returns `true`. |

---

| #     | Name                                                 | Description                                                                 |
|-------|------------------------------------------------------|-----------------------------------------------------------------------------|
| 10    | `ReferenceEquals()` with reference types             | Strict identity check — returns `true` only if both refer to the same instance. |
| 11    | `ReferenceEquals()` with boxed `int`                 | Boxing creates separate objects — even equal values return `false`.        |
| 12    | `ReferenceEquals()` with boxed struct                | Same as above — each struct boxed into a new object → returns `false`.     |