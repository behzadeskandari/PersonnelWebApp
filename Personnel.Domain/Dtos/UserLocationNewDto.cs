using Personnel.Domain.Entities.Identity;
using Personnel.Domain.MapperProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Dtos
{
    public class UserLocationNewDto : BaseEntityDto, ICreateMapper<UserLocation>
    {
        public string Title { get; set; }

        public int Code { get; set; }

        public int DisplayOrder { get; set; }
        public int ChargoonCode { get; set; }
        public int CiscoCode { get; set; }
        /// <summary>
        /// آیا براس سامانه جذب و استخدام فعال می باشد
        /// </summary>
        public bool IsActiveInRecruit { get; set; }
        /// <summary>
        /// آیا سازمانی هست
        /// </summary>
        public bool IsOprational { get; set; }

        public UserLocationType UserLocationType { get; set; }
    }
}
