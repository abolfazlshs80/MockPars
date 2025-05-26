using MockPars.Application.DTO.@base;
using MockPars.Application.DTO.RecordData;

namespace MockPars.Application.DTO.Column;

public record ColumnItemDto(int Id,string ColumnName, string ColumnType, FakeDataTypesDto FakeDataTypes,int TableId, IEnumerable<RecordDataItemDto> RecordData);