namespace Epistle.Models;

public class DocumentDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string ObjectsCollectionName { get; set; } = null!;
}
