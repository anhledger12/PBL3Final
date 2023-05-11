using Microsoft.EntityFrameworkCore;
using PBL3.Models.Entities;

namespace PBL3.Data
{
    public partial class QL
    {
        // Quản lý Hastag, viết sơn làm
        public List<Hashtag> GetAllHashtags()
        {
            return _context.Hashtags.ToList();
        }
        // Hàm lấy đủ tham chiếu các sách có hashtag id
        public Hashtag? GetHashtagFullByID(int id)
        {
            return _context.Hashtags.Include(p=>p.IdTitles).SingleOrDefault(p=>p.Id==id);
        }
        public Hashtag? GetHashtagFullByName(string name)
        {
            return _context.Hashtags.Include(p => p.IdTitles).SingleOrDefault(p => p.NameHashTag == name);
        }
        // Phương thức lấy danh sách các sách trong trang thứ page, số lượng từng trang là quantity

        public async Task AddHashtagToTitleAsync(string hashtag, string idtitle)
        {
            var ht = _context.Hashtags.Where(p => p.NameHashTag == hashtag).FirstOrDefault();
            var tt = _context.Titles.Where(p => p.IdTitle == idtitle).FirstOrDefault();
            tt.IdHashtags.Add(ht);
            _context.Titles.Update(tt);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteHashtagFromTitleAsync(string hashtag, string idtitle)
        {
            var ht = _context.Hashtags.Where(p => p.NameHashTag == hashtag).FirstOrDefault();
            var tt = _context.Titles.Include(p=>p.IdHashtags).Where(p => p.IdTitle == idtitle).FirstOrDefault();
            tt.IdHashtags.Remove(ht);
            _context.Titles.Update(tt);
            await _context.SaveChangesAsync();
        }

    }
}
