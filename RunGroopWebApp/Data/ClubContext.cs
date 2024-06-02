using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Data
{
    public class ClubContext : IdentityDbContext<AppUser>
    {
        public ClubContext(DbContextOptions<ClubContext> options) : base(options)
        {

        }
        public DbSet<Race> Races { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Address> Addresses { get; set; }
        //public DbSet<State> States { get; set; }
        //public DbSet<City> Cities { get; set; }
    }
}

//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using RunGroopWebApp.Data.Enum;
//using RunGroopWebApp.Models;

//namespace RunGroopWebApp.Data
//{
//    public class ClubContext : IdentityDbContext<AppUser>
//    {
//        public ClubContext(DbContextOptions<ClubContext> options) : base(options) 
//        {

//        } // end constructor

//        public DbSet<Race> Races { get; set; }
//        public DbSet<Club> Clubs { get; set; }
//        public DbSet<Address> Addresses { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            // Seed Clubs
//            modelBuilder.Entity<Club>().HasData(
//                new Club
//                {
//                    Id = 1,
//                    Title = "Running Club 1",
//                    Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
//                    Description = "This is the description of the first cinema",
//                    ClubCategory = ClubCategory.City,
//                    AddressId = 1
//                },
//                new Club
//                {
//                    Id = 2,
//                    Title = "Running Club 2",
//                    Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
//                    Description = "Join us for group runs and training sessions",
//                    ClubCategory = ClubCategory.Endurance,
//                    AddressId = 1
//                },
//                new Club
//                {
//                    Id = 3,
//                    Title = "Running Club 3",
//                    Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
//                    Description = "This is the description of the first club",
//                    ClubCategory = ClubCategory.Trail,
//                    AddressId = 1
//                },
//                new Club
//                {
//                    Id = 4,
//                    Title = "Running Club 4",
//                    Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
//                    Description = "This is the description of the first club",
//                    ClubCategory = ClubCategory.City,
//                    AddressId = 2
//                }
//            );

//            // Seed Races
//            modelBuilder.Entity<Race>().HasData(
//                new Race
//                {
//                    Id = 1,
//                    Title = "Running Race 1",
//                    Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
//                    Description = "This is the description of the first race",
//                    RaceCategory = RaceCategory.Marathon,
//                    AddressId = 1
//                },
//                new Race
//                {
//                    Id = 2,
//                    Title = "Running Race 2",
//                    Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
//                    Description = "This is the description of the first race",
//                    RaceCategory = RaceCategory.Ultra,
//                    AddressId = 1
//                }
//            );

//            // Seed Addresses
//            modelBuilder.Entity<Address>().HasData(
//                new Address
//                {
//                    Id = 1,
//                    Street = "123 Main St",
//                    City = "Charlotte",
//                    State = "NC"
//                },
//                new Address
//                {
//                    Id = 2,
//                    Street = "456 Elm St",
//                    City = "Michigan",
//                    State = "NC"
//                }
//            );
//        }
//    }
//}

