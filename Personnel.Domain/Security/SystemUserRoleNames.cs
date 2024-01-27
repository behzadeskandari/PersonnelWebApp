using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Security
{
    public static partial class SystemUserRoleNames
    {
        public static string Administrators { get { return "Administrator"; } }

        public static string Registered { get { return "Registered"; } }
        public static string Guests { get { return "Guests"; } }
        public static string Personel { get { return "Personel"; } }
        public static string Manager { get { return "Manager"; } }
        public static string DeputyCEO { get { return "DeputyCEO"; } }
        public static string CEO { get { return "CEO"; } }
        public static string Programmer { get { return "Programmer"; } }
    }
}
