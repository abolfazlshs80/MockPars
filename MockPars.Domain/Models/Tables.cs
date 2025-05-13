namespace MockPars.Domain.Models;

public class Tables
{
    public int Id { get; set; }
    public string TableName { get; set; }

    public string Slug { get; set; }
    public bool IsGetAll { get; set; }
    public bool IsGet { get; set; }
    public bool IsPut { get; set; }
    public bool IsPost { get; set; }
    public bool IsDelete { get; set; }
    public int DatabaseId { get; set; }
    public Databases Databases { get; set; }

    public HashSet<Columns> Columns { get; set; }
}