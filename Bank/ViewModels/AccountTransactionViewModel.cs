using System;

namespace Bank.ViewModels
{
    public class AccountTransactionViewModel
    {
        public int AccountId { get; set; }
        public string Frequency { get; set; }
        public DateTime Created { get; set; }
        public decimal Balance { get; set; }
    }
}