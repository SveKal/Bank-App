using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Bank.Models;
using Bank.Repositories.Interfaces;
using Bank.ViewModels;

namespace Bank.Repositories.Classes
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BankAppDataContext _context;

        public TransactionRepository(BankAppDataContext context)
        {
            _context = context;
        }
        public Accounts GetAccountById(int id)
        {
            return _context.Accounts.FirstOrDefault(x => x.AccountId == id);
        }
        public bool AccountExists(int id)
        {
            var account = _context.Accounts.Where(a => a.AccountId == id).SingleOrDefault();

            if (account == null)
            {
                return false;
            }

            return true;
        }
        public void Update(Accounts account)
        {
            _context.Update(account);
            _context.SaveChanges();
        }
        public bool HasCoverage(decimal balance, decimal amount)
        {
            if (balance - amount < 0)
            {
                return false;
            }
            return true;
        }
        public bool SameAccount(int accountId, int toAccountId)
        {
            if (accountId == toAccountId)
            {
                return false;
            }
            return true;
        }
        public void CreateTransaction(Transactions transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }
    }
}