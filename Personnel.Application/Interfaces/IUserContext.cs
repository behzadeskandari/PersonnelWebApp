using Personnel.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Application.Interfaces
{
    public interface IUserContext
    {
        User CurrentUser { get; }
        Task SetCurrentUser();
    }
}
