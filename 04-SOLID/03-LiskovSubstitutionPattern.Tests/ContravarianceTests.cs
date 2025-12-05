namespace _03_LiskovSubstitutionPattern.Tests;

public interface IContravariant<in T>
{
    void Set(T value);
}

public class CarSetter : IContravariant<Car>
{
    private protected Car? _car;
    public virtual void Set(Car value)
        => _car = value;
}

// Trying to override the Set method with a more specific signature would be a compile-time error
public class SportsCarSetter : CarSetter
{
    // public override void Set(SportsCar value)
    // {
    //     _car = value;
    // }
}

public class ContravarianceTests
{
    [Test]
    public void ContravarianceTest()
    {
        IContravariant<Car> carSetter = new CarSetter();
        
        // Contravariance: A consumer of Car can be assigned to a consumer of SportsCar
        // Why? Because if it can handle ANY Car, it can certainly handle a SportsCar
        // This assignment only works because of the "in" keyword in the interface!
        IContravariant<SportsCar> sportsCarSetter = carSetter;
        Assert.That(sportsCarSetter, Is.SameAs(carSetter));
        
        // carSetter accepts the entire hierarchy: Car > SportsCar > Convertible
        carSetter.Set(new Car());
        carSetter.Set(new SportsCar());
        carSetter.Set(new Convertible());
        
        // sportsCarSetter (same instance!) is typed to only accept SportsCar and subtypes
        // Passing a plain Car here would be a compile-time error
        sportsCarSetter.Set(new SportsCar());
        sportsCarSetter.Set(new Convertible());
    }
}
