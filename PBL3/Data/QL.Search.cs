using PBL3.Models.Entities;

namespace PBL3.Data
{
    public partial class QL
    {
        public List<Account> GetAccounts(string name, bool stateActive = true)
        {
           return _context.Accounts
                        .Select(x => x)
                        .Where(x => (x.AccName.Contains(name) || x.Email.Contains(name)) && x.Active == stateActive).ToList();
        }
        public List<Title> GetTitlesByName(string name, bool stateActive = true)
        {
            return _context.Titles
                         .Select(x => x)
                         .Where(x => x.NameBook.Contains(name) && x.Active == stateActive).ToList();
        }
    }
}
