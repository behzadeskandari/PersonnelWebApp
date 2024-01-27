using Personnel.Domain.Entities.Identity;
using Personnel.Domain.MapperProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Dtos.Users
{
    public class UserLocationNewDto : BaseEntityDto, ICreateMapper<UserLocation>
    {
        public string Title { get; set; }

        public int Code { get; set; }

        public int DisplayOrder { get; set; }
        public int ChargoonCode { get; set; }
        public int CiscoCode { get; set; }
        public bool IsActiveInRecruit { get; set; }
        public bool IsOprational { get; set; }

        public UserLocationType UserLocationType { get; set; }
    }
}
