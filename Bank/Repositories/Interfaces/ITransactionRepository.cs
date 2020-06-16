using System;
using Bank.Models;
using Bank.ViewModels;

namespace Bank.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        public Accounts GetAccountById(int id);
        bool AccountExists(int id);

        public bool Transfer(TransactionViewModel viewModel, Accounts fromAccount, Accounts toAccount,
            Transactions fromTransaction, Transactions toTransaction);
        public bool Deposit(TransactionViewModel viewModel, Transactions transaction, Accounts account);
        public bool Withdrawal(TransactionViewModel viewModel, Transactions transaction, Accounts account);
        public bool CreateTransaction(TransactionViewModel viewModel, Transactions transaction, Accounts account);
        public bool HasCoverage(decimal balance, decimal amount);
        public void Update(Accounts account);
        public bool SameAccount(int accountId, int toAccountId);
    }
}