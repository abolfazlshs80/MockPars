using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockPars.Application.DTO.RecordData
{
    public class CreateRecordDataDto
    {

        public int RowIndex { get; set; }
        public string Value { get; set; }
        public int ColumnsId { get; set; }

    }
}
