using MockPars.Application.DTO.@base;

namespace MockPars.Application.DTO.Database;

public class ColumnInfoDto
{
    public string Name { get; set; }
    public string TypeColumn { get; set; }
    public bool IsPrimaryKey { get; set; }
    public bool IsComputed { get; set; }
    public bool IsNullable { get; set; }


}