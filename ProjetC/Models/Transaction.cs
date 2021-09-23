using System;
using System.ComponentModel.DataAnnotations;

namespace ProjetC.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionNumber { get; set; }
        [Required]
        public int TransactionAmount { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        [Required]
        public int TransactionOrigin { get; set; }
        [Required]
        public int TransactionDestination { get; set; }
        [Required]
        public bool isValid { get; set; }
        public Transaction()
        {
            TransactionDate = DateTime.Now;
        }
    }
}
