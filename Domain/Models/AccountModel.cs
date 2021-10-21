using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class AccountModel
    {
        [Key]
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        [Required]
        public int AccountBalance { get; set; }
        public DateTime AccountCreationDate { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Utilisez seulement des lettres")]
        public string AccountHolderFirstName { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Utilisez seulement des lettres")]
        public string AccountHolderLastName { get; set; }
        public bool IsActive { get; set; }
        public AccountModel()
        {
            AccountBalance = 0;
            AccountCreationDate = DateTime.Now;
            IsActive = true;
        }
    }
}
