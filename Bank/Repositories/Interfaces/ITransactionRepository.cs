using System;
using Bank.Models;
using Bank.ViewModels;

namespace Bank.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        public Accounts GetAccountById(int id);
        bool AccountExists(int id);
        public void CreateTransaction(Transactions transaction);
        public bool HasCoverage(decimal balance, decimal amount);
        public void Update(Accounts account);
        public bool SameAccount(int accountId, int toAccountId);
    }
}