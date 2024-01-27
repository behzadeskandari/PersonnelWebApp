using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Personnel.Domain.Extensions;
using Personnel.Domain.Entities.Identity;
using Personnel.Infra.Data.Contracts;
using Personnel.Infra.Data.Mapping.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Infra.Data.Context
{
    public class PersonnelDbContext : IdentityDbContext<User, Roles, int, UserClaim, UserInRole, UserLogin, RoleClaim, SiteUserToken>, IDbContext
    {
        public PersonnelDbContext()
        {

        }

        public PersonnelDbContext(DbContextOptions options) : base(options)
        {
            // base.SaveChanges += OnSavingChanges;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RoleMap());
            modelBuilder.ApplyConfiguration(new UserTokenMap());
            modelBuilder.ApplyConfiguration(new UserInRoleMap());
            modelBuilder.ApplyConfiguration(new ManagerOfUserMap());
            modelBuilder.ApplyConfiguration(new PermissionRecordMap());
            modelBuilder.ApplyConfiguration(new PermissionInRoleMap());
            modelBuilder.ApplyConfiguration(new ClientTokenMap());
            modelBuilder.ApplyConfiguration(new SiteUserTokenMap());

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);


            //var builder = new ConfigurationBuilder();
            //var currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);  // Get the directory of the current assembly
            ////builder.SetBasePath(currentDir);
            ////builder.AddJsonFile("local.settings.json");
            //var configuration = builder.Build();
            
            //var connectionString = configuration.GetConnectionString("DefaultConnection");

            //optionsBuilder.UseSqlServer(connectionString);
        }
      

        public DatabaseFacade PersonnelDataBase => base.Database;

        //public DbSet<Course> Courses { get; set; }

      
    }
}
