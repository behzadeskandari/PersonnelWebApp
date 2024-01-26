using Personnel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Infra.Data.Mapping.Logging
{
    public class ActivityLog : BaseEntity
    {
        /// <summary>
        /// Gets or sets the activity log type identifier
        /// </summary>
        public string ActivityLogType { get; set; }

        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the activity comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        public string CreatedOnUtcString { get { return CreatedOnUtc > DateTime.MinValue ? CreatedOnUtc.ToString("f") : ""; } }

        /// <summary>
        /// Gets or sets the ip address
        /// </summary>
        public virtual string IpAddress { get; set; }
    }

}
