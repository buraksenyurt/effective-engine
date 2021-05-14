using Microsoft.EntityFrameworkCore;

namespace GalaxyExplorer.Entity
{
    public class GalaxyExplorerDbContext
        : DbContext
    {
        public GalaxyExplorerDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Spaceship> Spaceships { get; set; }
        public DbSet<Voyager> Voyagers { get; set; }
        public DbSet<Mission> Missions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mission>().HasMany(m => m.Voyagers).WithOne();

            modelBuilder.Entity<Spaceship>().HasData(
                new Spaceship
                {
                    SpaceshipId=1,
                    Name = "Saturn IV Rocket",
                    OnMission = false,
                    Range = 1.2,
                    MaxCrewCount=2
                },
                new Spaceship
                {
                    SpaceshipId = 2,
                    Name = "Pathfinder",
                    OnMission = true,
                    Range = 2.6,
                    MaxCrewCount = 5
                },
                new Spaceship
                {
                    SpaceshipId = 3,
                    Name = "Event Horizon",
                    OnMission = false,
                    Range = 9.9,
                    MaxCrewCount = 3
                },
                new Spaceship
                {
                    SpaceshipId = 4,
                    Name = "Captain Marvel",
                    OnMission = false,
                    Range = 3.14,
                    MaxCrewCount = 7
                },
                new Spaceship
                {
                    SpaceshipId = 5,
                    Name = "Lucky Tortiinn",
                    OnMission = false,
                    Range = 7.7,
                    MaxCrewCount = 7
                },
                new Spaceship
                {
                    SpaceshipId = 6,
                    Name = "Battle Master",
                    OnMission = false,
                    Range = 10,
                    MaxCrewCount = 5
                },
                new Spaceship
                {
                    SpaceshipId = 7,
                    Name = "Zerash Guidah",
                    OnMission = true,
                    Range = 3.35,
                    MaxCrewCount = 3
                },
                new Spaceship
                {
                    SpaceshipId = 8,
                    Name = "Ayran Hayd",
                    OnMission = false,
                    Range = 5.1,
                    MaxCrewCount = 4
                },
                new Spaceship
                {
                    SpaceshipId = 9,
                    Name = "Nebukadnezar",
                    OnMission = false,
                    Range = 9,
                    MaxCrewCount = 7
                },
                new Spaceship
                {
                    SpaceshipId = 10,
                    Name = "Sifiyus Alpha Siera",
                    OnMission = false,
                    Range = 7.7,
                    MaxCrewCount = 7
                }
            );
        }
    }
}
