using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class TransactionModel
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
        public bool IsValid { get; set; }
        public TransactionModel()
        {
            TransactionDate = DateTime.Now;
        }
    }
}
