using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Dtos
{
    public class ResultFilterDto
    {

        public object ExtraData { get; set; }

        public IEnumerable data { get; set; }

        public object Errors { get; set; }

        public int total { get; set; }

        public int recordsTotal { get { return total; } }
        public int recordsFiltered { get { return total; } }
    }
}
