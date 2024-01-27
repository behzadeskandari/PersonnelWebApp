using Personnel.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Interfaces
{
    public interface IPermissionService
    {
        Task<bool> HasPermission(int userId, string permissionSystemName);


        bool Authorize(int userId, PermissionRecord permission);

        bool Authorize(int userId, string permissionSystemName);

        PermissionRecord GetBySystemName(string systemName);

        IList<PermissionRecord> AllPermissionRecord();

    }
}
