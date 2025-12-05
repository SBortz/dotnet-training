namespace _04_InterfaceSegregationPrinciple;

using Bad = _04_InterfaceSegregationPrinciple.BadExample;
using Good = _04_InterfaceSegregationPrinciple.GoodExample;

public class InterfaceSegregationTests
{
    [Test]
    public void BadExample_Robot_ForcedToImplementMethodsItCantUse()
    {
        Bad.IWorker robot = new Bad.Robot();
        
        robot.Work(); // Works fine
        
        // Robot is forced to have Eat/Sleep but can't actually do them
        Assert.Throws<NotSupportedException>(() => robot.Eat());
        Assert.Throws<NotSupportedException>(() => robot.Sleep());
    }

    [Test]
    public void GoodExample_SimplePrinter_OnlyImplementsWhatItNeeds()
    {
        var printer = new Good.SimplePrinter();
        
        printer.Print("Hello");
        printer.Print("World");
        
        Assert.That(printer.PrintedDocuments, Has.Count.EqualTo(2));
        
        // SimplePrinter doesn't need to implement IScanner or IFax
        Assert.That(printer, Is.Not.InstanceOf<Good.IScanner>());
        Assert.That(printer, Is.Not.InstanceOf<Good.IFax>());
    }
}

