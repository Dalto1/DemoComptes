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
        public int TransactionSource { get; set; }
        [Required]
        public int TransactionDestination { get; set; }
    }
}
