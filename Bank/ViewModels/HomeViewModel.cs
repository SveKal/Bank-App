using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Bank.Models;

namespace Bank.ViewModels
{
    public class HomeViewModel
    {
        public ICollection<Accounts> Accounts { get; set; }
        public ICollection<Customers> Customers { get; set; }
        public int AmountCustomers { get; set; }
        public int AmountAccounts { get; set; }
        public decimal SumAccounts { get; set; }
    }
}
