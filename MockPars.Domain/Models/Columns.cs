using MockPars.Domain.Enums;

namespace MockPars.Domain.Models;

public class Columns
{
    public int Id { get; set; }

    public string ColumnName { get; set; }
    public string ColumnType { get; set; }

    public FakeDataTypes FakeDataTypes { get; set; }
    public int TablesId { get; set; }
    public Tables Tables { get; set; }

}