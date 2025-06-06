﻿using MockPars.Application.DTO.@base;
using MockPars.Domain.Enums;

namespace MockPars.Application.DTO.Column;

public class UpdateColumnDto
{
    public int Id { get; set; }

    public string ColumnName { get; set; }
    public string ColumnType { get; set; }

    public FakeDataTypesDto FakeDataTypes { get; set; }
    public int TableId { get; set; }
}