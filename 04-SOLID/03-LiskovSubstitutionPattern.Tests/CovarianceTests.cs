namespace _03_LiskovSubstitutionPattern.Tests;



public interface ICovariant<out T>
{
    T Get();
}

public class SportsCarGetter : ICovariant<SportsCar>
{
    private static readonly SportsCar _instance = new();
    public SportsCar Get() => _instance;
}


public class CovarianceTests
{
    [Test]
    public void Covariance_Test()
    {
        // SportsCarGetter implements ICovariant<SportsCar> - it returns SportsCar
        ICovariant<SportsCar> sportsCarGetter = new SportsCarGetter();
        
        // Covariance: A producer of SportsCar can safely be used as a producer of Car
        // Because every SportsCar IS a Car - the consumer will always get a valid Car
        // This assignment only works because of the "out" keyword in the interface!
        ICovariant<Car> carGetter = sportsCarGetter;
        Assert.That(sportsCarGetter, Is.SameAs(carGetter));
        
        // sportsCarGetter.Get() returns SportsCar (specific type)
        SportsCar sportsCar = sportsCarGetter.Get();
        
        // carGetter.Get() is typed as Car, but the actual instance is still a SportsCar
        Car car = carGetter.Get();
        
        // Both variables point to the same SportsCar instance
        Assert.That(sportsCar, Is.TypeOf<SportsCar>());
        Assert.That(car, Is.TypeOf<SportsCar>());  // Still a SportsCar at runtime!
        Assert.That(sportsCar, Is.Not.Null);
        Assert.That(car, Is.Not.Null);
    }
}
