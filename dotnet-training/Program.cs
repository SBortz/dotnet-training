// -------------------------------
// -------------- == -------------
// -------------------------------

// ----- == with reference types (when not overriden) -----
// 'a1' and 'a2' are two different PersonA instances with the same data.
// But since == compares references here, this returns false.
Console.WriteLine("1. == with reference types:");
PersonA a1 = new PersonA { Name = "Alice" };
PersonA a2 = new PersonA { Name = "Alice" };
Console.WriteLine(a1 == a2); // False

// ----- == with built-in value types -----
// For built-in value types like int, == is defined and compares values directly.
// Since 42 == 42, the result is true.
Console.WriteLine("\n1a. == with int (value type):");
int i1 = 42;
int i2 = 42;
Console.WriteLine(i1 == i2); // True

// ----- == with custom value types (struct) -----
// For structs like MyStruct, C# does not define the == operator by default.
// Trying to compare them using == causes a compile-time error unless you overload it.
Console.WriteLine("\n2. == with value types (undefined operator):");
MyStruct s1 = new MyStruct { X = 1 };
MyStruct s2 = new MyStruct { X = 1 };
// Console.WriteLine(s1 == s2); // ❌ Uncommenting this line causes a compiler error




// -------------------------------
// ---------- .Equals() ----------
// -------------------------------

// ----- .Equals() with reference types (not overridden) -----
// The default implementation of Equals (from System.Object) compares references.
// Even if the content is the same, two different instances return false.
Console.WriteLine("\n3. .Equals() with reference types:");
PersonA a3 = new PersonA { Name = "Alice" };
PersonA a4 = new PersonA { Name = "Alice" };
Console.WriteLine(a3.Equals(a4)); // False

// ----- .Equals() with int (built-in value type) -----
// For value types like int, Equals() checks for value equality.
// The result is true since 100 == 100.
Console.WriteLine("\n3a. .Equals() with int (value type):");
int j1 = 100;
int j2 = 100;
Console.WriteLine(j1.Equals(j2)); // True

// ----- .Equals() with custom value types (struct) ----- 
// Structs in C# perform field-by-field comparison by default via reflection.
// Here, both points have the same values → Equals returns true.
Console.WriteLine("\n4. .Equals() with value types:");
Point point1 = new Point { X = 1, Y = 2 };
Point point2 = new Point { X = 1, Y = 2 };
Console.WriteLine(point1.Equals(point2)); // True





// ----- == used for reference identity (typical use case) -----
// test1 and test2 refer to the same object, so == returns true.
// test3 is a new object, so == returns false even if it's of the same type.
Console.WriteLine("\n5. == with reference equality:");
Test test1 = new Test();
Test test2 = test1;
Test test3 = new Test();
Console.WriteLine(test1 == test2); // True
Console.WriteLine(test1 == test3); // False

// ----- [6] .Equals() overridden for value equality -----
// The PersonB class overrides Equals to compare values (here: Name).
// This allows semantically correct equality based on object content.
Console.WriteLine("\n6. .Equals() overridden:");
PersonB b1 = new PersonB { Name = "Alice" };
PersonB b2 = new PersonB { Name = "Alice" };
Console.WriteLine(b1.Equals(b2)); // True

// ----- [7] == operator overloaded for value equality -----
// The PersonC class overloads == to check for Name equality.
// This makes == behave like Equals, allowing cleaner syntax for comparisons.
Console.WriteLine("\n7. == overloaded for value comparison:");
PersonC c1 = new PersonC { Name = "Alice" };
PersonC c2 = new PersonC { Name = "Alice" };
Console.WriteLine(c1 == c2); // True
Console.WriteLine(c1.Equals(c2)); // True – uses overloaded Equals which calls ==

// ----- [8] Object.ReferenceEquals: strict identity check ----- 
// This method always checks for actual object identity (memory address),
// and is guaranteed not to be affected by operator overloading or Equals overrides.
Console.WriteLine("\n8. Object.ReferenceEquals always compares identity:");
object o1 = new object();
object o2 = o1;
object o3 = new object();
Console.WriteLine(Object.ReferenceEquals(o1, o2)); // True
Console.WriteLine(Object.ReferenceEquals(o1, o3)); // False



// Simple class without Equals or operator overloads
class PersonA
{
    public string Name;
}

// Struct with no == operator defined
struct MyStruct
{
    public int X;
}

// Value-type struct where default Equals does field-wise comparison
struct Point
{
    public int X, Y;
}

// Basic reference type for identity checks
class Test { }

// Class with overridden Equals and GetHashCode for value equality
class PersonB
{
    public string Name;

    public override bool Equals(object obj)
    {
        return obj is PersonB p && p.Name == this.Name;
    }

    public override int GetHashCode() => Name.GetHashCode();
}

// Class with full equality logic: operator overloading, Equals and GetHashCode
class PersonC
{
    public string Name;

    public static bool operator ==(PersonC a, PersonC b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        return a.Name == b.Name;
    }

    public static bool operator !=(PersonC a, PersonC b) => !(a == b);

    public override bool Equals(object obj) => obj is PersonC p && this == p;

    public override int GetHashCode() => Name?.GetHashCode() ?? 0;
}