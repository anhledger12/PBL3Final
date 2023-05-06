using Microsoft.AspNetCore.Mvc;
using PBL3.Models.Entities;

namespace PBL3.Models
{
    public class SearchViewModel
    {
        public IEnumerable<Title> Titles { get; set; }
        public IEnumerable<Account> Accounts { get; set; }
    }
}
