# 🧪 C# Equality Overview

This is a list of all examples included in `Program.cs`, with a short explanation for each.

---

## 🔹 `==` Operator Comparison Behavior

The `==` operator in C# behaves differently depending on the type: for value types, it usually compares the actual values, while for reference types, it compares references (memory addresses) by default. Some types like `record` or `string` have an overloaded `==` operator that performs a value comparison instead. The table below summarizes the default behavior for various types:

| Type          | Category          | `==` Default Behavior | Notes |
|---------------|-------------------|-------|-----------------------------------------------------------------------|
| `int`    			| value		 	| **values** | 																		|
| `struct` 			| value		 	| - 		| Leads to compiler error, when `==` operator is not explicitly overriden.	|
| `record struct` 	| value 		| **values** | Compiler generates implementation for `==`, `!=`, `Equals()`, `GetHashCode()` and `ToString()` |
| `class`  			| reference	 	| **references** | 																	|
| `record` 			| reference	 	| **values** | Compiler generates implementation for `==`, `!=`, `Equals()`, `GetHashCode()` and `ToString()` |
| `string` 			| reference	 	| **values** |  Its overloaded `==` operator first checks reference equality (fast path) and then falls back to value comparison. String interning makes the reference path hit more often. |

---

## 🔹 `.Equals()` Comparison behavior

The `.Equals()` method is mostly intended for **deep value content comparison**. For value types, it compares values by default. But for reference types you have to override `.Equals()`, otherwise you will get reference comparison.

| Type          | Category          | `.Equals()` Default Behavior | Notes |
|---------------|-------------------|-----------|---------------------------------------------------------------------------------------|
| `int`    			| value		 	| **values** 	| 																						|
| `struct` 			| value		 	| **values** 	| Compares field-by-field using ValueType.Equals. Older .NET versions used reflection; newer one generate IL, but it's still slower than a custom IEquatable<T> implementation. |
| `record struct` 	| value	 	 	| **values** 	| Compiler generates implementation for `==`, `!=`, `Equals()`, `GetHashCode()` and `ToString()` -> Faster than `struct`	|
| `class`  			| reference	 	| **references** | 																						|
| `record class` (short: `record`) 	| reference	 		| **values** 	| Compiler generates implementation for `Equals()`, `GetHashCode()` and `ToString()`.	|
| `string` 			| reference	 	| **values** 	| .Equals() compares values directly. String interning is not used in this case. 		|

---

## 🔹 `ReferenceEquals()` Identity Checks

`Object.ReferenceEquals()` checks if two variables point to the **exact same object in memory**. It is unaffected by overrides or boxing logic and is useful when you want to ensure **true identity**.


| #     | Name                                                 | Description                                                                 |
|-------|------------------------------------------------------|-----------------------------------------------------------------------------|
| 10    | `ReferenceEquals()` with reference types             | Strict identity check — returns `true` only if both refer to the same instance. |
| 11    | `ReferenceEquals()` with boxed `int`                 | Boxing creates separate objects — even equal values would return `false`.        |
| 12    | `ReferenceEquals()` with boxed struct                | Same as above — each struct boxed into a new object → returns `false`.     |
