namespace _04_InterfaceSegregationPrinciple.GoodExample;

public class SimplePrinter : IPrinter
{
    public List<string> PrintedDocuments { get; } = [];

    public void Print(string document) => PrintedDocuments.Add(document);
}

