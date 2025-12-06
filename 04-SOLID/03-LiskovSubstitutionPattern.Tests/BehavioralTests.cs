namespace _03_LiskovSubstitutionPattern.Tests;

// LSP Behavioral Rule: Subclasses must not throw exceptions
// that are not expected by the base class contract.
//
// Here, SuperClass.Do() throws SuperException.
// Consumer code (TestUseCase) catches SuperException.
// SubClassBreak violates LSP by throwing AnotherException instead.

[TestFixture]
public class BehavioralTests
{
    [Test]
    public void SuperClass_ThrowsSuperException()
    {
        var sut = new TestUseCase(new SuperClass());
        sut.ExecuteUseCase();
    }

    [Test]
    public void SubClassOk_ThrowsSuperException_BecauseSubExceptionInheritsFromIt()
    {
        var sut = new TestUseCase(new SubClassOk());
        sut.ExecuteUseCase();
    }

    [Test]
    public void SubClassBreak_ViolatesLSP_ThrowsUnexpectedException()
    {
        var sut = new TestUseCase(new SubClassBreak());
        
        // Throws AnotherException which is not caught by the consumer
        Assert.Throws<AnotherException>(() => sut.ExecuteUseCase());
    }
}

/// <summary>
/// Simulates consumer code that depends on SuperClass behavior.
/// </summary>
public class TestUseCase(SuperClass superClass)
{
    public void ExecuteUseCase()
    {
        try
        {
            superClass.Do();
        }
        catch (SuperException)
        {
            // expected, handled
        }
    }
}

public class SuperClass
{
public virtual void Do()
    => throw new SuperException();
}

public class SuperException : Exception { }

public class SubClassOk : SuperClass
{
    public override void Do()
        => throw new SubException();
}
public class SubException : SuperException { }

public class SubClassBreak : SuperClass
{
    public override void Do()
        => throw new AnotherException();
}
public class AnotherException : Exception { }