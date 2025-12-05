namespace _04_InterfaceSegregationPrinciple.GoodExample;

public class MultiFunctionDevice : IPrinter, IScanner, IFax
{
    public List<string> PrintedDocuments { get; } = [];
    public List<string> ScannedDocuments { get; } = [];
    public List<(string Document, string Number)> SentFaxes { get; } = [];

    public void Print(string document) => PrintedDocuments.Add(document);
    
    public string Scan()
    {
        var doc = $"Scanned_{ScannedDocuments.Count + 1}";
        ScannedDocuments.Add(doc);
        return doc;
    }

    public void Fax(string document, string number) => SentFaxes.Add((document, number));
}

