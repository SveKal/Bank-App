using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Bank.Models;

namespace Bank.ViewModels
{
    public class CustomerListViewModel
    {
        public Customers Customer { get; set; }
        public ICollection<Accounts> Accounts { get; set; }
        [Required]
        public string SearchString { get; set; }
        public decimal TotalBalance => Accounts.Sum(a => a.Balance);
    }
}