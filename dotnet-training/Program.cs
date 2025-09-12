using System;

struct PointStruct { public int X; public int Y; }
public record struct PointRecordStruct(int X, int Y);
class Box { public int V; }
public record Person(string Name, int Age);

class Program
{
    static void Main()
    {
        // =====================================================================
        Console.WriteLine("== Operator – Default Comparison Behavior");
        // =====================================================================

        // 1) value | int | values
        Console.WriteLine("\n[== #1] Type=value, DataType=int, Default=values");
        Console.WriteLine("Expected: ints compare by value. 5 == 5 -> true.");
        int i1 = 5, i2 = 5;
        bool iEq = i1 == i2;
        Console.WriteLine($"Expression: 5 == 5");
        Console.WriteLine($"Result    : {iEq}");

        // 2) value | struct | - (compile error if not overloaded)
        Console.WriteLine("\n[== #2] Type=value, DataType=struct, Default=-");
        Console.WriteLine("Expected: custom structs don't have '==' defined -> compile error.");
        var s1 = new PointStruct { X = 1, Y = 2 };
        var s2 = new PointStruct { X = 1, Y = 2 };
        //var fail = s1 == s2;  // ❌ Operator '==' is not defined
        Console.WriteLine("Note      : This code is commented out because it wouldn't compile:");
        Console.WriteLine("Result    : n/a (compile-time error)");

        // 3) value | record struct | values
        Console.WriteLine("\n[== #3] Type=value, DataType=record struct, Default=values");
        Console.WriteLine("Expected: record struct has generated '==' operator comparing all fields.");
        var rs1 = new PointRecordStruct(1, 2);
        var rs2 = new PointRecordStruct(1, 2);
        bool rsEq = rs1 == rs2;
        Console.WriteLine("Expression: new PointRecordStruct(1,2) == new PointRecordStruct(1,2)");
        Console.WriteLine($"Result     : {rsEq}");

        // 4) reference | class | references
        Console.WriteLine("\n[== #4] Type=reference, DataType=class, Default=references");
        Console.WriteLine("Expected: classes use reference comparison with '=='. Different instances are not equal.");
        var c1 = new Box { V = 1 };
        var c2 = new Box { V = 1 };
        bool cEq = c1 == c2;
        Console.WriteLine("Expression: new Box{V=1} == new Box{V=1}");
        Console.WriteLine($"Result     : {cEq}");

        // 5) reference | record (record class) | values
        Console.WriteLine("\n[== #5] Type=reference, DataType=record (class), Default=values");
        Console.WriteLine("Expected: record class has generated '==' comparing by value.");
        var p1 = new Person("Anna", 30);
        var p2 = new Person("Anna", 30);
        bool recEq = p1 == p2;
        Console.WriteLine("Expression: new Person(\"Anna\",30) == new Person(\"Anna\",30)");
        Console.WriteLine($"Result     : {recEq}");

        // 6) reference | string | values (reference fast-path)
        Console.WriteLine("\n[== #6] Type=reference, DataType=string, Default=values");
        Console.WriteLine("Expected: '==' compares content, but first checks reference (fast-path). Interning makes this often true.");
        string str1 = "abc";
        string str2 = "abc";
        bool strEq = str1 == str2;
        Console.WriteLine("Expression: \"abc\" == \"abc\"");
        Console.WriteLine($"Result     : {strEq}");

        // =====================================================================
        Console.WriteLine("\n.Equals() – Default Comparison Behavior");
        // =====================================================================

        // 1) value | int | values
        Console.WriteLine("\n[Equals #1] Type=value, DataType=int, Default=values");
        Console.WriteLine("Expected: int.Equals compares values.");
        int e1 = 42, e2 = 42;
        bool eInt = e1.Equals(e2);
        Console.WriteLine("Expression: 42.Equals(42)");
        Console.WriteLine($"Result     : {eInt}");

        // 2) value | struct | values (field-by-field)
        Console.WriteLine("\n[Equals #2] Type=value, DataType=struct, Default=values");
        Console.WriteLine("Expected: struct.Equals compares all fields (ValueType.Equals).");
        var es1 = new PointStruct { X = 1, Y = 2 };
        var es2 = new PointStruct { X = 1, Y = 2 };
        bool eStruct = es1.Equals(es2);
        Console.WriteLine("Expression: new PointStruct{1,2}.Equals(new PointStruct{1,2})");
        Console.WriteLine($"Result     : {eStruct}");

        // 3) value | record struct | values (generated)
        Console.WriteLine("\n[Equals #3] Type=value, DataType=record struct, Default=values");
        Console.WriteLine("Expected: record struct has generated Equals; compares fields without reflection.");
        var ers1 = new PointRecordStruct(3, 4);
        var ers2 = new PointRecordStruct(3, 4);
        bool eRecStruct = ers1.Equals(ers2);
        Console.WriteLine("Expression: new PointRecordStruct(3,4).Equals(new PointRecordStruct(3,4))");
        Console.WriteLine($"Result     : {eRecStruct}");

        // 4) reference | class | references
        Console.WriteLine("\n[Equals #4] Type=reference, DataType=class, Default=references");
        Console.WriteLine("Expected: class.Equals without override uses reference comparison.");
        var ec1 = new Box { V = 7 };
        var ec2 = new Box { V = 7 };
        bool eClass = ec1.Equals(ec2);
        Console.WriteLine("Expression: new Box{7}.Equals(new Box{7})");
        Console.WriteLine($"Result     : {eClass}");

        // 5) reference | record class | values
        Console.WriteLine("\n[Equals #5] Type=reference, DataType=record class, Default=values");
        Console.WriteLine("Expected: record class has generated Equals; compares content.");
        var ep1 = new Person("Ben", 40);
        var ep2 = new Person("Ben", 40);
        bool eRecClass = ep1.Equals(ep2);
        Console.WriteLine("Expression: new Person(\"Ben\",40).Equals(new Person(\"Ben\",40))");
        Console.WriteLine($"Result     : {eRecClass}");

        // 6) reference | string | values
        Console.WriteLine("\n[Equals #6] Type=reference, DataType=string, Default=values");
        Console.WriteLine("Expected: string.Equals compares content (Ordinal), interning is irrelevant.");
        string esA = "Hello";
        string esB = new string("Hello".ToCharArray()); // different reference, same content
        bool eString = esA.Equals(esB);
        Console.WriteLine("Expression: \"Hello\".Equals(new string(\"Hello\".ToCharArray()))");
        Console.WriteLine($"Result     : {eString}");

        // =====================================================================
        Console.WriteLine("\nReferenceEquals() – Identity Checks");
        // =====================================================================

        // 10) ReferenceEquals with reference types
        Console.WriteLine("\n[RefEq #10] ReferenceEquals(ref types)");
        Console.WriteLine("Expected: only true if both variables reference the same object.");
        var r1 = new Person("Ida", 20);
        var r2 = new Person("Ida", 20);
        bool refTypes = Object.ReferenceEquals(r1, r2);
        Console.WriteLine("Expression: ReferenceEquals(new Person(\"Ida\",20), new Person(\"Ida\",20))");
        Console.WriteLine($"Result     : {refTypes}");

        // 11) ReferenceEquals with boxed int
        Console.WriteLine("\n[RefEq #11] ReferenceEquals(boxed int)");
        Console.WriteLine("Expected: each boxing creates a new object -> false.");
        int bx = 123;
        object bo1 = bx;
        object bo2 = bx;
        bool refBoxedInt = Object.ReferenceEquals(bo1, bo2);
        Console.WriteLine("Expression: var o1=(object)123; var o2=(object)123; ReferenceEquals(o1,o2)");
        Console.WriteLine($"Result     : {refBoxedInt}");

        // 12) ReferenceEquals with boxed struct
        Console.WriteLine("\n[RefEq #12] ReferenceEquals(boxed struct)");
        Console.WriteLine("Expected: each boxing of a struct creates a new object -> false.");
        var st = new PointStruct { X = 9, Y = 9 };
        object bs1 = st;
        object bs2 = st;
        bool refBoxedStruct = Object.ReferenceEquals(bs1, bs2);
        Console.WriteLine("Expression: var o1=(object)st; var o2=(object)st; ReferenceEquals(o1,o2)");
        Console.WriteLine($"Result     : {refBoxedStruct}");

        Console.WriteLine("\n-- End of demo --");
    }
}
