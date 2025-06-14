# 🔍 C# Equality & Identity Training

This project demonstrates how **equality (`==`, `.Equals()`)** and **identity (`ReferenceEquals`)** behave in C#. It covers reference types, value types, structs, operator overloading, boxing, and interning.

The code is structured into clear examples, each with an explanation and output.

---

## 🎯 Purpose

Even after many many years of acting in the .NET field I was recently in a situation where I wasn´t 100% sure any more about all the details and differences between `==` and `.Equals()`. So I took some time and created this overview to compare them.

---

# 🧪 List of Examples – Equality & Identity in C#

This is a list of all examples included in `Program.cs`, with a short explanation for each.

---

## 🔹 `==` Operator Examples

The `==` operator compares references by default for reference types and compares values for built-in value types (like `int`). It can be **overridden** in custom classes to support **semantic value equality**.


| #    | Name                                                  | Description                                                                 |
|------|-------------------------------------------------------|-----------------------------------------------------------------------------|
| 1    | `==` with reference types (not overridden)            | Compares two objects of the same class with identical content → `false` due to reference comparison. |
| 2    | `==` with reference types (overridden for value)      | Uses overloaded `==` in `PersonC` class to compare values semantically.     |
| 3    | `==` with built-in value types (`int`)                | Compares values directly → returns `true` for equal values.                 |
| 4    | `==` with `string`                                    | `string` overrides `==` to compare content, not reference.                 |
| 5    | `==` with custom value types (struct)                 | Not allowed unless explicitly overloaded → compile error otherwise.        |

---

## 🔹 `.Equals()` Method Examples

The `.Equals()` method is intended for **semantic content comparison**. For value types, it compares field-by-field. For reference types, you must **override** it to get meaningful value equality.


| #    | Name                                                  | Description                                                                 |
|------|-------------------------------------------------------|-----------------------------------------------------------------------------|
| 6    | `.Equals()` with reference types (not overridden)     | Default `Equals()` compares references → returns `false` for same content. |
| 7    | `.Equals()` with reference types (overridden)         | `PersonB` overrides `Equals()` to enable semantic value comparison.        |
| 8    | `.Equals()` with built-in value types (`int`)         | Built-in types override `Equals()` to compare values directly.             |
| 9    | `.Equals()` with custom struct                        | Structs support default field-wise comparison via `Equals()` → returns `true`. |

---

## 🔹 `ReferenceEquals()` Identity Checks

`Object.ReferenceEquals()` checks if two variables point to the **exact same object in memory**. It is unaffected by overrides or boxing logic and is useful when you want to ensure **true identity**.


| #     | Name                                                 | Description                                                                 |
|-------|------------------------------------------------------|-----------------------------------------------------------------------------|
| 10    | `ReferenceEquals()` with reference types             | Strict identity check — returns `true` only if both refer to the same instance. |
| 11    | `ReferenceEquals()` with boxed `int`                 | Boxing creates separate objects — even equal values return `false`.        |
| 12    | `ReferenceEquals()` with boxed struct                | Same as above — each struct boxed into a new object → returns `false`.     |
