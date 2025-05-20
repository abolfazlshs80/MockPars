namespace MockPars.Application.DTO.Column;

public record ColumnItemDto(int Id,string ColumnName, string ColumnType, FakeDataTypesDto FakeDataTypes,int TableId);