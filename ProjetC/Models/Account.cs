using System.ComponentModel.DataAnnotations;

namespace ProjetC.Models
{
    public class Account
    {
        [Key]
        public int AccountNumber { get; set; }
        [Required]
        public int AccountBalance { get; set; }
        [Required]
        public bool isActive { get; set; }
        public Account()
        {
            this.AccountBalance = 0;
            this.isActive = true;
        }
    }
}
