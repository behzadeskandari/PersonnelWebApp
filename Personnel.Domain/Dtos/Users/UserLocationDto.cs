using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Dtos.Users
{
    public class UserLocationDto : BaseEntityDto
    {
        public string Title { get; set; }
        public bool IsActiveInRecruit { get; set; }
        public int Code { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsOprational { get; set; }
    }
}
