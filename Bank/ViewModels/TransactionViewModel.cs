using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bank.ViewModels
{
    public class TransactionViewModel
    {
        public int TransactionId { get; set; }
        [Required(ErrorMessage = "Please enter an Account Number.")]
        public int AccountId { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Operation { get; set; }
        [Required(ErrorMessage = "Please enter an amount.")]
        [Range(1, 500000, ErrorMessage = "Amount can not be lower than 1 or higher than 50000. For higher transactions please contact the bank.")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public string Symbol { get; set; }
        public string Bank { get; set; }
        public string Account { get; set; }
        [Required(ErrorMessage = "Please enter an Account Number.")]
        [DisplayName("To Account")]
        public int ToAccountId { get; set; }
        public string Frequency { get; set; }
        public DateTime Created { get; set; }
            
    }
}