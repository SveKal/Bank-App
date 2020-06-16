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
        public bool Deposit(TransactionViewModel viewModel, Transactions transaction, Accounts account)
        {
            account.Balance += viewModel.Amount;
            Update(account);
            viewModel.Type = "Credit";
            CreateTransaction(viewModel, transaction, account);
            return true;
        }
        
        public bool Withdrawal(TransactionViewModel viewModel, Transactions transaction, Accounts account)
        {
            if (HasCoverage(viewModel.Balance, viewModel.Amount))
            {
                account.Balance -= viewModel.Amount;
                Update(account);
                viewModel.Amount = -viewModel.Amount;
                viewModel.Type = "Debit";
                CreateTransaction(viewModel, transaction, account);
                return true;
            }
            return false;
        }
        public bool CreateTransaction(TransactionViewModel viewModel, Transactions transaction, Accounts account)
        {
            transaction.AccountId = viewModel.AccountId;
            transaction.Date = DateTime.Now;
            transaction.Type = viewModel.Type;
            transaction.Operation = viewModel.Operation;
            transaction.Amount = viewModel.Amount;
            transaction.Balance = account.Balance;
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return true;
        }
        public bool Transfer(TransactionViewModel viewModel, Accounts fromAccount, Accounts toAccount, Transactions fromTransaction, Transactions toTransaction)
        {
            if (HasCoverage(fromAccount.Balance, viewModel.Amount))
            {
                var newVm = new TransactionViewModel();
                viewModel.Operation = "Transfer to other Bank";
                Withdrawal(viewModel, fromTransaction, fromAccount);
                newVm.AccountId = toAccount.AccountId;
                newVm.Operation = "Transfer from other Bank";
                newVm.Amount = -viewModel.Amount;
                newVm.Balance = toAccount.Balance;
                newVm.Date = DateTime.Now;
                newVm.Bank = viewModel.Bank;
                newVm.Symbol = viewModel.Symbol;
                newVm.Type = viewModel.Type;
                toTransaction.AccountId = toAccount.AccountId;
                Deposit(newVm, toTransaction, toAccount);
            }
            return false;
        }
    }
}