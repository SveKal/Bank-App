using System.Collections.Generic;
using Bank.Models;

namespace Bank.ViewModels
{
    public class AccountDetailsViewModel
    {
        public List<Transactions> Transactions { get; set; }
        public int AccountId { get; set; }
        public int PagesMax { get; set; }
        public int PageNow { get; set; }
    }
}