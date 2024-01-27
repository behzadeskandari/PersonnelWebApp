using Personnel.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Infra.Data.Interfaces.Users
{
    public interface IUserLocationRepository : IBaseRepository<UserLocation>
    {
        void Remove(int id);
        void AddUserLocation(UserLocation userLocation);
        void UpdateUserLocation(UserLocation userLocation);

        Task<List<UserLocation>> GetOprationalUserLocationsAsync();
    }
}
