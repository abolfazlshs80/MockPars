namespace MockPars.Application.DTO.Table;

public class UpdateTableDto
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
}