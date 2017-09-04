using Jorje.TheWorld.Dal.DomainModels;
using Jorje.TheWorld.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Jorje.TheWorld.Dal.Context
{
    public class WorldDBContext : DbContext
    {
        //private string _connectionString;
        //private ILoggerFactory _logger;

        private IConfigurationRoot _config;

        public WorldDBContext() : base()
        {
        }

        public WorldDBContext(IConfigurationRoot config) : base()
        {
            _config = config;

            //_connectionString = connectionString;
            //_logger = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDb;Database=TheWorld_Dev;Trusted_Connection=true;MultipleActiveResultSets=true;");

            //optionsBuilder.UseLoggerFactory(_logger);


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonTrip>().HasKey(s => new { s.TripId, s.PersonId });

            //modelBuilder.Entity<Person>().Property(s => s.PersonAdditionalData).IsRequired();

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entityType.Name).Property<DateTime>("LastModified");
            }

            
            
            //modelBuilder.Entity<StopDTO>(entity =>
            //{
            //    entity.HasKey(e => e.StopId).HasName("PK_Stop");

            //    entity.ToTable("Stop");

            //    entity.Property(e => e.StopId).HasColumnName("StopId");
            //    entity.Property(e => e.Name).HasColumnName("Name");
            //    entity.Property(e => e.Latitude).HasColumnName("Latitude");
            //    entity.Property(e => e.Longitude).HasColumnName("Longitude");
            //    entity.Property(e => e.Order).HasColumnName("Order");
            //    entity.Property(e => e.Arrival).HasColumnName("ArrivalTime");

            //    entity.Property(e => e.Name).HasColumnType("nvarchar(200)");
            //    entity.Property(e => e.Name).HasMaxLength(200);

            //});

            //modelBuilder.Entity<TripDto>(entity =>
            //{
            //    entity.HasKey(e => e.TripId).HasName("PK_Trip");

            //    entity.ToTable("Trip");

            //    entity.Property(e => e.TripId).HasColumnName("TripId");
            //    entity.Property(e => e.Name).HasColumnName("Name");
            //    entity.Property(e => e.UserName).HasColumnName("UserName");
            //    entity.Property(e => e.DateCreated).HasColumnName("DateCreated");
            //    //entity.Property(e => e.Stops).HasColumnName("StopId");

            //    entity.Property(e => e.Name).HasColumnType("nvarchar(200)");
            //    entity.Property(e => e.Name).HasMaxLength(200);

            //});
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                entry.Property("LastModified").CurrentValue = DateTime.Now;
            }

            return base.SaveChanges();
        }

        public virtual DbSet<Stop> Stops { get; set; }
        public virtual DbSet<Trip> Trips { get; set; }


    }
}
