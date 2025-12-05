namespace _04_InterfaceSegregationPrinciple.BadExample;

public interface IWorker
{
    void Work();
    void Eat();
    void Sleep();
}

public class Human : IWorker
{
    public List<string> Actions { get; } = [];

    public void Work() => Actions.Add("Human working");
    public void Eat() => Actions.Add("Human eating");
    public void Sleep() => Actions.Add("Human sleeping");
}

public class Robot : IWorker
{
    public List<string> Actions { get; } = [];

    public void Work() => Actions.Add("Robot working");
    
    // Robot forced to implement methods it can't use
    public void Eat() => throw new NotSupportedException("Robots don't eat");
    public void Sleep() => throw new NotSupportedException("Robots don't sleep");
}

