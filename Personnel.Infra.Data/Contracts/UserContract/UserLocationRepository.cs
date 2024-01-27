using Microsoft.EntityFrameworkCore;
using Personnel.Domain.Entities.Identity;
using Personnel.Infra.Data.Context;
using Personnel.Infra.Data.Interfaces.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Infra.Data.Contracts.UserContract
{
    public class UserLocationRepository : BaseAsyncRepository<UserLocation>, IUserLocationRepository
    {
        public UserLocationRepository(PersonnelDbContext dbContext) : base(dbContext)
        {
        }

        public void Remove(int id)
        {
            var userLocation = Entities.Find(id);
            if (userLocation != null)
                Entities.Remove(userLocation);
        }

        public void AddUserLocation(UserLocation userLocation)
        {
            Entities.Add(userLocation);
        }

        public void UpdateUserLocation(UserLocation userLocation)
        {
            Entities.Update(userLocation);
        }

        public async Task<List<UserLocation>> GetOprationalUserLocationsAsync()
        {
            var query = await Entities
                .AsNoTracking()
                .Where(x => x.IsOprational).ToListAsync();
            return query;
        }
    }
}
