using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;


/////////////////////////////////////////////////////////////
/// This Repository uses the IRaceInterface and is injected /
/// into the project with dependency inkection in the       /
/// program.cs file.                                        / 
/////////////////////////////////////////////////////////////
///
namespace RunGroopWebApp.Repository
{
    public class RaceRepository : IRaceRepository
    {
        private readonly ClubContext Context;
        public RaceRepository(ClubContext context) { Context = context; }

        public bool Add(Race race)
        {
            Context.Add(race);
            return Save();
        }

        public bool Delete(Race race)
        {
            Context.Remove(race);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetAll()
        {
            return await Context.Races.ToListAsync();
        }

        public async Task<IEnumerable<Race>> GetAllRacesByCity(string city)
        {
            return await Context.Races.Where(r => r.Address.City.Contains(city)).ToListAsync();
        }

        public async Task<Race> GetByIdAsync(int id)
        {
            return await Context.Races.Include(a => a.Address).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Race> GetByIdAsyncNoTracking(int id)
        {
            return await Context.Races.Include(a => a.Address).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public bool Save()
        {
            var saved = Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Race race)
        {
            Context.Update(race);
            return Save();
        }

    }
}
