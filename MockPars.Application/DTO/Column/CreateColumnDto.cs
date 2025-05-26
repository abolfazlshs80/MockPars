using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MockPars.Application.DTO.@base;

namespace MockPars.Application.DTO.Column
{
    public class CreateColumnDto
    {

        public string ColumnName { get; set; }
        public string ColumnType { get; set; }

        public FakeDataTypesDto FakeDataTypes { get; set; }
        public int TableId { get; set; }
    }
}
