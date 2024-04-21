namespace Epistle.Domain;

public class Owner : Person
{
    public override string? Type { get => "Person"; }

    public string? Salt { get; set; }

    public string? Password { get; set; }
}
