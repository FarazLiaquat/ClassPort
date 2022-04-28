using ClassPort.Infrastructure.Persistence.DbContexts;
using RoverCore.Abstractions.Seeder;
using ClassPort.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassPort.Infrastructure.Pass.Seeding
{
    public class LocationSeeder : ISeeder
    {
        private readonly ApplicationDbContext _context;
        public int Priority => 0;

        public LocationSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (_context.Location.Any())
                return;

            _context.Location.Add(new Location { Place = "Gym", RoomNum = "1" }); _context.Location.Add(new Location { Place = "Library", RoomNum = "2" }); _context.Location.Add(new Location { Place = "Lunch Room", RoomNum = "3" }); _context.Location.Add(new Location { Place = "Bathroom", RoomNum = "Hallway" });
            await _context.SaveChangesAsync();
        }

    }
}