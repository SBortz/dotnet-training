namespace _03_LiskovSubstitutionPattern.Tests;

public record Car { }
public record SportsCar : Car { }

public record Convertible : SportsCar { }