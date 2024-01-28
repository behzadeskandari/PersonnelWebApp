using Personnel.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
        string GetToken(User user);
    }
}
