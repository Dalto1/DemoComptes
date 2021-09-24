using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace REST.Models
{
    public class Account
    {
        [Key]
        public int AccountNumber { get; set; }
        [Required]
        public int AccountBalance { get; set; }
        [Required]
        public DateTime AccountCreationDate { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Utilisez seulement des lettres")]
        public string AccountHolderFirstName { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Utilisez seulement des lettres")]
        public string AccountHolderLastName { get; set; }
        [Required]
        public bool isActive { get; set; }
        public Account()
        {
            AccountBalance = 0;
            AccountCreationDate = DateTime.Now;
        }
    }
}
