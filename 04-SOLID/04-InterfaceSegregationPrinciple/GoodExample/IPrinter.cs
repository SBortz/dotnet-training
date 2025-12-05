namespace _04_InterfaceSegregationPrinciple.GoodExample;

public interface IPrinter
{
    void Print(string document);
}

public interface IScanner
{
    string Scan();
}

public interface IFax
{
    void Fax(string document, string number);
}

