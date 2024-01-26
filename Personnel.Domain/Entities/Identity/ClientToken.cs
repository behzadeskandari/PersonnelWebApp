using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Entities.Identity
{
    public class ClientToken : BaseEntity
    {
        public int Token { get; set; }
        public string Mobile { get; set; }
        public string NationalCode { get; set; }
        public DateTime CreateDate { get; set; }
        public string Guid { get; set; }
    }
}
