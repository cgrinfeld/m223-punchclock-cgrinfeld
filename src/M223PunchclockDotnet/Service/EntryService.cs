using M223PunchclockDotnet.Model;
using Microsoft.EntityFrameworkCore;

namespace M223PunchclockDotnet.Service
{
    public class EntryService
    {
        private DatabaseContext _databaseContext; 

        public EntryService(DatabaseContext databaseContext){
            _databaseContext = databaseContext;
        }
        public Task<List<Entry>> FindAll()
        {
            return _databaseContext.Entries.ToListAsync();
        }

        public Task<Entry?> FindById(int id)
        {
            return _databaseContext.Entries.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task DeleteEntry(int id)
        {
            var entry = await _databaseContext.Entries.FindAsync(id);
            if (entry != null)
            {
                _databaseContext.Entries.Remove(entry);
                await _databaseContext.SaveChangesAsync();
            }
        }

        public async Task UpdateEntry(Entry entry)
        {
            _databaseContext.Entries.Update(entry);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<Entry> AddEntry(Entry entry)
        {
            _databaseContext.Entries.Add(entry);
            await _databaseContext.SaveChangesAsync();

            return entry;
        }
    }
}
