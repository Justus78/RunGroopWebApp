using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository
{
    public class ClubRepository : IClubRepository
    {
        public readonly ClubContext Context;

        public ClubRepository(ClubContext context) 
        { 
            Context = context;
        } // end constructor
        public bool Add(Club club)
        {
            Context.Add(club);
            return Save();
        } // end add method

        public bool Delete(Club club)
        {
            Context.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAll()
        {
            return await Context.Clubs.ToListAsync();
        }

        public async Task<Club> GetByIdAsync(int id)
        {
            return await Context.Clubs.Include(a => a.Address).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Club> GetByIdAsyncNoTracking(int id)
        {
            return await Context.Clubs.Include(a => a.Address).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            return await Context.Clubs.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
            var saved = Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Club club)
        {
            Context.Update(club);
            return Save();
        }
    }
}
