using MockPars.Domain.Enums;

namespace MockPars.Application.DTO.RecordData;

public class UpdateRecordDataDto
{
    public int Id { get; set; }

    public string Value { get; set; }
    public int ColumnsId { get; set; }
}