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

This is a complete overview of the examples in `Program.cs`.

| #   | Name                                             | Description                                                                |
|-----|--------------------------------------------------|----------------------------------------------------------------------------|
| 1   | `==` with reference types (not overridden)       | Shows that `==` compares references by default for classes.                |
| 2   | `==` with built-in value types (`int`)           | Compares actual numeric values — always true for equal numbers.            |
| 3   | `==` with `string`                               | `string` overrides `==` for value-based content comparison.                |
| 4   | `==` with custom structs                         | Fails to compile unless operator is overloaded — not defined by default.   |
| 5   | `.Equals()` with reference types (not overridden)| Default `Equals()` compares references like `==` unless overridden.        |
| 6   | `.Equals()` with reference types (overridden)    | Shows how to define logical content equality by overriding `Equals()`.     |
| 7   | `.Equals()` with built-in value types            | Value types like `int` override `.Equals()` for content comparison.        |
| 8   | `.Equals()` with structs                         | Structs support structural equality out of the box via field-by-field compare. |
| 9   | `==` operator overloaded (value equality)        | Demonstrates custom `==` logic for semantic comparison of objects.         |
| 10  | `ReferenceEquals()` with reference types         | Checks if two variables point to the exact same object in memory.          |
| 11  | `ReferenceEquals()` with boxed `int`             | Boxing creates new objects — even for equal values, identity differs.      |
| 12  | `ReferenceEquals()` with boxed struct            | Like with `int`, boxing structs results in different references.           |
