using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MockPars.Application.DTO.Database;

namespace MockPars.Application.DTO.Table
{
    public class FakeDataToTableDto
    {
        public ConnectionDatabaseDto Database { get; set; }
        public List<TableInfoDto> Tables { get; set; }
        public int Count { get; set; }
    }
    public class GetColumnByTableDto
    {
        public ConnectionDatabaseDto Database { get; set; }
      public  TableInfoDto Table { get; set; }
   
    }
}
