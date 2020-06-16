using System;
using System.ComponentModel.DataAnnotations;

namespace Bank.ViewModels
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        [Required]
        [MaxLength(6)]
        public string Gender { get; set; }
        [Required]
        [MaxLength(100)]
        public string Givenname { get; set; }
        [Required]
        [MaxLength(100)]
        public string Surname { get; set; }
        [Required]
        [MaxLength(100)]
        public string Streetaddress { get; set; }
        [Required]
        [MaxLength(100)]
        public string City { get; set; }
        [Required]
        [MaxLength(15)]
        public string Zipcode { get; set; }
        [Required]
        [MaxLength(100)]
        public string Country { get; set; }
        [Required]
        [MaxLength(2)]
        public string CountryCode { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }
        [MaxLength(20)]
        public string NationalId { get; set; }
        [Required]
        [MaxLength(10)]
        public string Telephonecountrycode { get; set; }
        [Required]
        [MaxLength(25)]
        public string Telephonenumber { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Emailaddress { get; set; }

    }
}