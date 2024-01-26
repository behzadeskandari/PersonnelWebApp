using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Entities.Identity
{
    public class ManagerOfUser : BaseEntity
    {
        /// <summary>
        /// کد کاربر
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// کد مدیر کاربر
        /// </summary>
        public int UserManagerId { get; set; }
        public DateTime CreateDate { get; set; }
        public int RegisterUserId { get; set; }
    }
}
