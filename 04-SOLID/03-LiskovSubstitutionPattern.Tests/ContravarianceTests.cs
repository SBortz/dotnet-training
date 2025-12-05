namespace _03_LiskovSubstitutionPattern.Tests;
public class WeaponSetter : IContravariant<Weapon>
{
    private Weapon? _weapon;
    public void Set(Weapon value)
        => _weapon = value;
}

public interface IContravariant<in T>
{
    void Set(T value);
}

public class ContravarianceTests
{
    [Test]
    public void ContravarianceTest()
    {
        IContravariant<Weapon> weaponSetter = new WeaponSetter();
        IContravariant<Sword> swordSetter = weaponSetter;
        Assert.That(swordSetter, Is.SameAs(weaponSetter));
        // Contravariance: Weapon > Sword > TwoHandedSword
        weaponSetter.Set(new Weapon());
        weaponSetter.Set(new Sword());
        weaponSetter.Set(new TwoHandedSword());
        // Contravariance: Sword > TwoHandedSword
        swordSetter.Set(new Sword());
        swordSetter.Set(new TwoHandedSword());
    }
}