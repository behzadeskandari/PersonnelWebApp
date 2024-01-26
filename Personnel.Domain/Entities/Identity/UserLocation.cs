using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Entities.Identity
{
    public class UserLocation : BaseEntity
    {
        public string Title { get; set; }

        public int Code { get; set; }

        public int DisplayOrder { get; set; }
        
        public bool IsActiveInRecruit { get; set; }
      
        public bool IsOprational { get; set; }

        public UserLocationType UserLocationType { get; set; }

    }

    public enum UserLocationType : byte
    {
        None = 0,
        Technical = 1,
    }
}
