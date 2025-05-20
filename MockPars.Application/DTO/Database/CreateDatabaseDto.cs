using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockPars.Application.DTO.Database
{
    public class CreateDatabaseDto
    {
        public string DatabaseName { get; set; }

        public string Slug { get; set; }
        public string UserId { get; set; }
    }
}
