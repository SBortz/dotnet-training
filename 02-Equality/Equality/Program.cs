using System;
using Equality.Tests.TestTypes;
using Point = System.Drawing.Point;

namespace DotnetTraining
{
    // Test classes and structures for equality tests
    

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("🧪 C# Equality Overview - Practical Examples\n");
            Console.WriteLine(new string('=', 60) + "\n");

            // 1. == Operator Tests
            TestEqualityOperator();
            
            // 2. .Equals() Tests
            TestEqualsMethod();
            
            // 3. ReferenceEquals() Tests
            TestReferenceEquals();
        }

        static void TestEqualityOperator()
        {
            Console.WriteLine("🔹 == Operator Default Comparison Behavior\n");
            
            // Value Types - int
            Console.WriteLine("📌 Value Types (int):");
            int a = 5;
            int b = 5;
            int c = 10;
            Console.WriteLine($"  int a = {a}, b = {b}, c = {c}");
            Console.WriteLine($"  a == b: {a == b} (values compared)");
            Console.WriteLine($"  a == c: {a == c} (values compared)");
            Console.WriteLine();

            // Value Types - struct (führt zu Compiler-Fehler, daher kommentiert)
            Console.WriteLine("📌 Value Types (struct):");
            Point point1 = new Point(1, 2);
            Point point2 = new Point(1, 2);
            Console.WriteLine($"  Point point1 = ({point1.X}, {point1.Y})");
            Console.WriteLine($"  Point point2 = ({point2.X}, {point2.Y})");
            Console.WriteLine("  point1 == point2: COMPILER ERROR - == operator not defined for struct");
            Console.WriteLine("  (Structs require explicit == operator override)");
            Console.WriteLine();

            // Value Types - record struct
            Console.WriteLine("📌 Value Types (record struct):");
            RecordPoint rp1 = new RecordPoint(1, 2);
            RecordPoint rp2 = new RecordPoint(1, 2);
            RecordPoint rp3 = new RecordPoint(3, 4);
            Console.WriteLine($"  RecordPoint rp1 = ({rp1.X}, {rp1.Y})");
            Console.WriteLine($"  RecordPoint rp2 = ({rp2.X}, {rp2.Y})");
            Console.WriteLine($"  RecordPoint rp3 = ({rp3.X}, {rp3.Y})");
            Console.WriteLine($"  rp1 == rp2: {rp1 == rp2} (values compared - compiler generated)");
            Console.WriteLine($"  rp1 == rp3: {rp1 == rp3} (values compared - compiler generated)");
            Console.WriteLine();

            // Reference Types - class
            Console.WriteLine("📌 Reference Types (class):");
            Person person1 = new Person("Alice", 30);
            Person person2 = new Person("Alice", 30);
            Person person3 = person1;
            Console.WriteLine($"  Person person1 = new Person(\"Alice\", 30)");
            Console.WriteLine($"  Person person2 = new Person(\"Alice\", 30)");
            Console.WriteLine($"  Person person3 = person1");
            Console.WriteLine($"  person1 == person2: {person1 == person2} (references compared)");
            Console.WriteLine($"  person1 == person3: {person1 == person3} (references compared)");
            Console.WriteLine();

            // Reference Types - record
            Console.WriteLine("📌 Reference Types (record):");
            PersonRecord pr1 = new PersonRecord("Bob", 25);
            PersonRecord pr2 = new PersonRecord("Bob", 25);
            PersonRecord pr3 = new PersonRecord("Charlie", 35);
            Console.WriteLine($"  PersonRecord pr1 = new PersonRecord(\"Bob\", 25)");
            Console.WriteLine($"  PersonRecord pr2 = new PersonRecord(\"Bob\", 25)");
            Console.WriteLine($"  PersonRecord pr3 = new PersonRecord(\"Charlie\", 35)");
            Console.WriteLine($"  pr1 == pr2: {pr1 == pr2} (values compared - compiler generated)");
            Console.WriteLine($"  pr1 == pr3: {pr1 == pr3} (values compared - compiler generated)");
            Console.WriteLine();

            // Reference Types - string
            Console.WriteLine("📌 Reference Types (string):");
            string str1 = "Hello";
            string str2 = "Hello";
            string str3 = "World";
            Console.WriteLine($"  string str1 = \"Hello\"");
            Console.WriteLine($"  string str2 = \"Hello\"");
            Console.WriteLine($"  string str3 = \"World\"");
            Console.WriteLine($"  str1 == str2: {str1 == str2} (values compared - overloaded operator)");
            Console.WriteLine($"  str1 == str3: {str1 == str3} (values compared - overloaded operator)");
            Console.WriteLine("  (String interning makes reference comparison often faster)");
            Console.WriteLine();

            Console.WriteLine(new string('=', 60) + "\n");
        }

        static void TestEqualsMethod()
        {
            Console.WriteLine("🔹 .Equals() Default Comparison Behavior\n");
            
            // Value Types - int
            Console.WriteLine("📌 Value Types (int):");
            int a = 5;
            int b = 5;
            int c = 10;
            Console.WriteLine($"  int a = {a}, b = {b}, c = {c}");
            Console.WriteLine($"  a.Equals(b): {a.Equals(b)} (values compared)");
            Console.WriteLine($"  a.Equals(c): {a.Equals(c)} (values compared)");
            Console.WriteLine();

            // Value Types - struct
            Console.WriteLine("📌 Value Types (struct):");
            Point point1 = new Point(1, 2);
            Point point2 = new Point(1, 2);
            Point point3 = new Point(3, 4);
            Console.WriteLine($"  Point point1 = ({point1.X}, {point1.Y})");
            Console.WriteLine($"  Point point2 = ({point2.X}, {point2.Y})");
            Console.WriteLine($"  Point point3 = ({point3.X}, {point3.Y})");
            Console.WriteLine($"  point1.Equals(point2): {point1.Equals(point2)} (values compared - ValueType.Equals)");
            Console.WriteLine($"  point1.Equals(point3): {point1.Equals(point3)} (values compared - ValueType.Equals)");
            Console.WriteLine("  (Slower than IEquatable<T> implementation)");
            Console.WriteLine();

            // Value Types - record struct
            Console.WriteLine("📌 Value Types (record struct):");
            RecordPoint rp1 = new RecordPoint(1, 2);
            RecordPoint rp2 = new RecordPoint(1, 2);
            RecordPoint rp3 = new RecordPoint(3, 4);
            Console.WriteLine($"  RecordPoint rp1 = ({rp1.X}, {rp1.Y})");
            Console.WriteLine($"  RecordPoint rp2 = ({rp2.X}, {rp2.Y})");
            Console.WriteLine($"  RecordPoint rp3 = ({rp3.X}, {rp3.Y})");
            Console.WriteLine($"  rp1.Equals(rp2): {rp1.Equals(rp2)} (values compared - compiler generated)");
            Console.WriteLine($"  rp1.Equals(rp3): {rp1.Equals(rp3)} (values compared - compiler generated)");
            Console.WriteLine("  (Faster than struct - compiler generated)");
            Console.WriteLine();

            // Reference Types - class
            Console.WriteLine("📌 Reference Types (class):");
            Person person1 = new Person("Alice", 30);
            Person person2 = new Person("Alice", 30);
            Person person3 = person1;
            Console.WriteLine($"  Person person1 = new Person(\"Alice\", 30)");
            Console.WriteLine($"  Person person2 = new Person(\"Alice\", 30)");
            Console.WriteLine($"  Person person3 = person1");
            Console.WriteLine($"  person1.Equals(person2): {person1.Equals(person2)} (references compared - default)");
            Console.WriteLine($"  person1.Equals(person3): {person1.Equals(person3)} (references compared - default)");
            Console.WriteLine("  (Must be overridden for value comparison)");
            Console.WriteLine();

            // Reference Types - record
            Console.WriteLine("📌 Reference Types (record):");
            PersonRecord pr1 = new PersonRecord("Bob", 25);
            PersonRecord pr2 = new PersonRecord("Bob", 25);
            PersonRecord pr3 = new PersonRecord("Charlie", 35);
            Console.WriteLine($"  PersonRecord pr1 = new PersonRecord(\"Bob\", 25)");
            Console.WriteLine($"  PersonRecord pr2 = new PersonRecord(\"Bob\", 25)");
            Console.WriteLine($"  PersonRecord pr3 = new PersonRecord(\"Charlie\", 35)");
            Console.WriteLine($"  pr1.Equals(pr2): {pr1.Equals(pr2)} (values compared - compiler generated)");
            Console.WriteLine($"  pr1.Equals(pr3): {pr1.Equals(pr3)} (values compared - compiler generated)");
            Console.WriteLine();

            // Reference Types - string
            Console.WriteLine("📌 Reference Types (string):");
            string str1 = "Hello";
            string str2 = "Hello";
            string str3 = "World";
            Console.WriteLine($"  string str1 = \"Hello\"");
            Console.WriteLine($"  string str2 = \"Hello\"");
            Console.WriteLine($"  string str3 = \"World\"");
            Console.WriteLine($"  str1.Equals(str2): {str1.Equals(str2)} (values compared directly)");
            Console.WriteLine($"  str1.Equals(str3): {str1.Equals(str3)} (values compared directly)");
            Console.WriteLine("  (String interning is NOT used here)");
            Console.WriteLine();

            Console.WriteLine(new string('=', 60) + "\n");
        }

        static void TestReferenceEquals()
        {
            Console.WriteLine("🔹 ReferenceEquals() Identity Checks\n");
            
            // Reference Types
            Console.WriteLine("📌 ReferenceEquals() mit Reference Types:");
            Person person1 = new Person("Alice", 30);
            Person person2 = new Person("Alice", 30);
            Person person3 = person1;
            Console.WriteLine($"  Person person1 = new Person(\"Alice\", 30)");
            Console.WriteLine($"  Person person2 = new Person(\"Alice\", 30)");
            Console.WriteLine($"  Person person3 = person1");
            Console.WriteLine($"  ReferenceEquals(person1, person2): {ReferenceEquals(person1, person2)} (different objects)");
            Console.WriteLine($"  ReferenceEquals(person1, person3): {ReferenceEquals(person1, person3)} (same reference)");
            Console.WriteLine();

            // Boxed int
            Console.WriteLine("📌 ReferenceEquals() mit boxed int:");
            int value1 = 42;
            int value2 = 42;
            object boxed1 = value1;
            object boxed2 = value2;
            object boxed3 = value1;
            Console.WriteLine($"  int value1 = {value1}, value2 = {value2}");
            Console.WriteLine($"  object boxed1 = value1, boxed2 = value2, boxed3 = value1");
            Console.WriteLine($"  ReferenceEquals(boxed1, boxed2): {ReferenceEquals(boxed1, boxed2)} (different box objects)");
            Console.WriteLine($"  ReferenceEquals(boxed1, boxed3): {ReferenceEquals(boxed1, boxed3)} (different box objects)");
            Console.WriteLine("  (Boxing creates separate objects - even with equal values)");
            Console.WriteLine();

            // Boxed struct
            Console.WriteLine("📌 ReferenceEquals() mit boxed struct:");
            Point point1 = new Point(1, 2);
            Point point2 = new Point(1, 2);
            object boxedPoint1 = point1;
            object boxedPoint2 = point2;
            object boxedPoint3 = point1;
            Console.WriteLine($"  Point point1 = ({point1.X}, {point1.Y})");
            Console.WriteLine($"  Point point2 = ({point2.X}, {point2.Y})");
            Console.WriteLine($"  object boxedPoint1 = point1, boxedPoint2 = point2, boxedPoint3 = point1");
            Console.WriteLine($"  ReferenceEquals(boxedPoint1, boxedPoint2): {ReferenceEquals(boxedPoint1, boxedPoint2)} (different box objects)");
            Console.WriteLine($"  ReferenceEquals(boxedPoint1, boxedPoint3): {ReferenceEquals(boxedPoint1, boxedPoint3)} (different box objects)");
            Console.WriteLine("  (Each struct boxing creates new object → false)");
            Console.WriteLine();

            Console.WriteLine(new string('=', 60) + "\n");
            Console.WriteLine("✅ All equality examples successfully demonstrated!");
        }
    }
}
