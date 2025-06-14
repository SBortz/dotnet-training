// -----------------------------------------------------------------------
// -----                                                             -----
// -----                             ==                              -----
// -----                                                             -----
// -----------------------------------------------------------------------
Console.WriteLine("----------------------------------------------------------------------");
Console.WriteLine("== operator");
Console.WriteLine("-----------------------------------------------------------------------");

// == with reference types - when not overriden
// 'a1' and 'a2' are two different PersonA instances with the same data.
// But since == compares references here, this returns false.
Console.WriteLine("== with reference types - when not overriden:");
PersonA a1 = new PersonA { Name = "Alice" };
PersonA a2 = new PersonA { Name = "Alice" };
Console.WriteLine(a1 == a2); // False


// == with reference types - overriden for value equality
// The PersonC class overloads == to check for Name equality.
// This makes == behave like Equals.
// Might make sense for DDD-style Value Objects.
Console.WriteLine("== with reference types - overriden for value equality:");
PersonC c1 = new PersonC { Name = "Alice" };
PersonC c2 = new PersonC { Name = "Alice" };
Console.WriteLine(c1 == c2); // True
Console.WriteLine(c1.Equals(c2)); // True – uses overloaded Equals which calls ==

// == with built-in value types (int, decimal, double, float)
// For built-in value types like int, == is defined and compares values directly.
// Since 42 == 42, the result is true.
Console.WriteLine("== with built-in value types (int, decimal, double, float):");
int i1 = 42;
int i2 = 42;
Console.WriteLine(i1 == i2); // True

// For string, although it's a reference type, == is overridden to call string.Equals
// string equals compares reference equality for interned strings (performance optimization)
// and content equality for non-interned strings.
string string1 = "hello";
string string2 = new string("hello".ToCharArray());
Console.WriteLine("For string, although it's a reference type, == is overridden to call string.Equals:"); 
Console.WriteLine(string1 == string2); 

// == with custom value types (struct)
// For structs like MyStruct, C# does not define the == operator by default.
// Trying to compare them using == causes a compile-time error unless you overload it.
Console.WriteLine("== with value types (undefined operator):");
MyStruct s1 = new MyStruct { X = 1 };
MyStruct s2 = new MyStruct { X = 1 };
// Console.WriteLine(s1 == s2); // ❌ Uncommenting this line causes a compiler error




// -----------------------------------------------------------------------
// -----                                                             -----
// -----                         .Equals()                           -----
// -----                                                             -----
// -----------------------------------------------------------------------
Console.WriteLine("\n\n-----------------------------------------------------------------------");
Console.WriteLine(".Equals()");
Console.WriteLine("-----------------------------------------------------------------------");

// .Equals() with reference types - not overridden
// The default implementation of Equals (from System.Object) compares references.
// Even if the content is the same, two different instances return false.
// Still .Equals purpose is to be explicitly overriden for custom content comparisons.
Console.WriteLine(".Equals() with reference types - not overridden:");
PersonA a3 = new PersonA { Name = "Alice" };
PersonA a4 = new PersonA { Name = "Alice" };
Console.WriteLine(a3.Equals(a4)); // False

// .Equals() with reference types - overridden
// The PersonB class overrides Equals to compare values (here: Name).
// This allows semantically correct equality based on object content.
Console.WriteLine(".Equals() with reference types - overridden:");
PersonB b1 = new PersonB { Name = "Alice" };
PersonB b2 = new PersonB { Name = "Alice" };
Console.WriteLine(b1.Equals(b2)); // True

// .Equals() with built-in value types (int, decimal, double, float)
// For value types like int, Equals() checks for value equality.
// The result is true since 100 == 100.
Console.WriteLine(".Equals() with built-in value types (int, decimal, double, float):");
int j1 = 100;
int j2 = 100;
Console.WriteLine(j1.Equals(j2)); // True

// .Equals() with custom value types (struct) 
// Structs in C# perform field-by-field comparison by default via reflection.
// Here, both points have the same values → Equals returns true.
Console.WriteLine(".Equals() with custom value types (struct)");
Point point1 = new Point { X = 1, Y = 2 };
Point point2 = new Point { X = 1, Y = 2 };
Console.WriteLine(point1.Equals(point2)); // True



// -----------------------------------------------------------------------
// -----                                                             -----
// -----                      .ReferenceEquals()                     -----
// -----                                                             -----
// -----------------------------------------------------------------------
Console.WriteLine("\n\n-----------------------------------------------------------------------");
Console.WriteLine(".ReferenceEquals()");
Console.WriteLine("-----------------------------------------------------------------------");

// Object.ReferenceEquals on reference types strict identity check 
// This method always checks for actual object identity (memory address),
// and is guaranteed not to be affected by operator overloading or Equals overrides.
Console.WriteLine("Object.ReferenceEquals on reference types strict identity check:");
object o1 = new object();
object o2 = o1;
object o3 = new object();
Console.WriteLine(Object.ReferenceEquals(o1, o2)); // True
Console.WriteLine(Object.ReferenceEquals(o1, o3)); // False

// Object.ReferenceEquals with int (value type)
// Value types are boxed when cast to object. Boxing creates a new object each time.
// Even if the values are equal, they are not the same reference.
int i5 = 42;
int i6 = 42;
object boxed1 = i5;
object boxed2 = i6;

Console.WriteLine("Object.ReferenceEquals with int (value type):");
Console.WriteLine(Object.ReferenceEquals(boxed1, boxed2)); // False – boxed separately


// Object.ReferenceEquals with struct (value type)
// Same logic applies for custom structs. Even if content is identical,
// each boxing operation creates a new object.
MyStruct ms1 = new MyStruct { X = 5 };
MyStruct ms2 = new MyStruct { X = 5 };
object boxedMs1 = ms1;
object boxedMs2 = ms2;

Console.WriteLine("Object.ReferenceEquals with struct (value type):");
Console.WriteLine(Object.ReferenceEquals(boxedMs1, boxedMs2)); // False – separate boxed instances





// Simple class without Equals or operator overloads
class PersonA
{
    public string? Name;
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
    public string? Name;

    public override bool Equals(object? obj)
    {
        return obj is PersonB p && p.Name == this.Name;
    }

    public override int GetHashCode() => Name.GetHashCode();
}

// Class with full equality logic: operator overloading, Equals and GetHashCode
class PersonC
{
    public string? Name;

    public static bool operator ==(PersonC? a, PersonC? b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        return a.Name == b.Name;
    }

    public static bool operator !=(PersonC a, PersonC b) => !(a == b);

    public override bool Equals(object? obj) => obj is PersonC p && this == p;

    public override int GetHashCode() => Name?.GetHashCode() ?? 0;
}