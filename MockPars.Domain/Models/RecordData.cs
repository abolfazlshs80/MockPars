namespace MockPars.Domain.Models;

public class RecordData
{
    public int Id { get; set; }

    public string Value { get; set; }
    public int ColumnsId { get; set; }
    public int RowIndex { get; set; }

    public Columns Columns { get; set; }
    
}