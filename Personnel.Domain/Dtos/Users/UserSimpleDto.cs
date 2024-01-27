using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Dtos.Users
{

    public class UserSimpleDto
    {
        public string FullNameAndOperationUnitCode { get; set; }
        public string OperationUnitCode { get; set; }
    }
}
