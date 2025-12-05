namespace _03_LiskovSubstitutionPattern.Tests;

public record class Weapon { }
public record class Sword : Weapon { }

public record class TwoHandedSword : Sword { }

public interface ICovariant<out T>
{
    T Get();
}

public class SwordGetter : ICovariant<Sword>
{
    private static readonly Sword _instance = new();
    public Sword Get() => _instance;
}


public class Tests
{
    [Test]
    public void Covariance_Test()
    {
        ICovariant<Sword> swordGetter = new SwordGetter();
        ICovariant<Weapon> weaponGetter = swordGetter;
        Assert.That(swordGetter, Is.SameAs(weaponGetter));
        Sword sword = swordGetter.Get();
        Weapon weapon = weaponGetter.Get();
        Assert.That(sword, Is.TypeOf<Sword>());
        Assert.That(weapon, Is.TypeOf<Sword>());
        Assert.That(sword, Is.Not.Null);
        Assert.That(weapon, Is.Not.Null);
    }
}