using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Jorje.TheWorld.Dal.Context;

namespace Jorje.TheWorld.Dal.Migrations
{
    [DbContext(typeof(WorldDBContext))]
    [Migration("20170829191429_PersonTrip")]
    partial class PersonTrip
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Jorje.TheWorld.Domain.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.HasKey("Id");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("Jorje.TheWorld.Domain.PersonTrip", b =>
                {
                    b.Property<int>("TripId");

                    b.Property<int>("PersonId");

                    b.HasKey("TripId", "PersonId");

                    b.HasIndex("PersonId");

                    b.ToTable("PersonTrip");
                });

            modelBuilder.Entity("Jorje.TheWorld.Domain.Stop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Arrival");

                    b.Property<decimal>("Latitude");

                    b.Property<decimal>("Longitude");

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.Property<int?>("TripId");

                    b.HasKey("Id");

                    b.HasIndex("TripId");

                    b.ToTable("Stops");
                });

            modelBuilder.Entity("Jorje.TheWorld.Domain.Trip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Name");

                    b.Property<int>("StopId");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("Jorje.TheWorld.Domain.PersonTrip", b =>
                {
                    b.HasOne("Jorje.TheWorld.Domain.Person", "Person")
                        .WithMany("PersonTrips")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Jorje.TheWorld.Domain.Trip", "Trip")
                        .WithMany("PersonTrips")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Jorje.TheWorld.Domain.Stop", b =>
                {
                    b.HasOne("Jorje.TheWorld.Domain.Trip")
                        .WithMany("Stops")
                        .HasForeignKey("TripId");
                });
        }
    }
}
