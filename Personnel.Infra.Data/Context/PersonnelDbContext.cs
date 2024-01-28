using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Personnel.Domain.Entities.Identity;
using Personnel.Infra.Data.Contracts;
using Personnel.Infra.Data.CrossCutting.Logging;
using Personnel.Infra.Data.Mapping.Identity;
using Personnel.Infra.Data.Mapping.Logging;
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
        public PersonnelDbContext(DbContextOptions<PersonnelDbContext> options) : base(options)
        {
            // base.SaveChanges += OnSavingChanges;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new RoleMap());
            modelBuilder.ApplyConfiguration(new UserTokenMap());
            modelBuilder.ApplyConfiguration(new UserInRoleMap());
            modelBuilder.ApplyConfiguration(new ManagerOfUserMap());
            modelBuilder.ApplyConfiguration(new PermissionRecordMap());
            modelBuilder.ApplyConfiguration(new PermissionInRoleMap());
            modelBuilder.ApplyConfiguration(new ClientTokenMap());
            modelBuilder.ApplyConfiguration(new SiteUserTokenMap());
            modelBuilder.ApplyConfiguration(new UserClaimMap());
            modelBuilder.ApplyConfiguration(new UserRefreshTokenMap());

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=PersonnelDB2;MultipleActiveResultSets=true;TrustServerCertificate=True;Integrated Security=SSPI");
            }
            optionsBuilder.EnableSensitiveDataLogging();
            MyLoggerProvider logFac = new MyLoggerProvider();
            //optionsBuilder.UseLoggerFactory(logFac);

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


    public class MyLoggerFactory : ILoggerFactory
    {
        private readonly MyLoggerProvider _provider = new MyLoggerProvider();

        public void AddProvider(ILoggerProvider provider)
        {
            // You might want to handle multiple providers here, 
            // but for simplicity, we'll just use a single provider.
            _provider.AddProvider(provider);
        }

        public ILogger CreateLogger(string categoryName)
        {
            // Your logic for creating a logger instance
            return _provider.CreateLogger(categoryName);
        }

        public void Dispose()
        {
            // Cleanup logic, if any
        }
    }


    public class MyLoggerProvider : ILoggerFactory, IDisposable
    {

        private readonly List<ILoggerProvider> _providers;
        public MyLoggerProvider()
        {
            _providers = new List<ILoggerProvider>();
        }

        public void AddProvider(ILoggerProvider provider)
        {
            _providers.Add(provider);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DatabaseLogger();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
    public class PersonnelDbContextFactory : IDesignTimeDbContextFactory<PersonnelDbContext>
    {
        public PersonnelDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PersonnelDbContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=PersonnelDB2;MultipleActiveResultSets=true;TrustServerCertificate=True;Integrated Security=SSPI");

            return new PersonnelDbContext(optionsBuilder.Options);
        }
    }
}
