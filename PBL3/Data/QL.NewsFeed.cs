using PBL3.Models.Entities;

namespace PBL3.Data
{
    public partial class QL
    {
        public IQueryable<NewsFeed> GetNewsFeeds()
        {
            return _context.NewsFeeds.OrderByDescending(p => p.Id);
        }
        public NewsFeed? GetNewsFeed(int id)
        {
            return _context.NewsFeeds.Where(p=>p.Id==id).FirstOrDefault();
        }
        public async Task AddNewsFeed(NewsFeed newsFeed)
        {
            _context.NewsFeeds.Add(newsFeed);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateNewsFeed(NewsFeed newsFeed)
        {
            _context.NewsFeeds.Update(newsFeed);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveNewsFeed(NewsFeed? newsFeed)
        {
            if (newsFeed != null)
            {
                _context.NewsFeeds.Remove(newsFeed);
            }
            await _context.SaveChangesAsync();
        }
    }
}
