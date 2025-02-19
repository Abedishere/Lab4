using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmindLab3LibraryDB.Models
{
    public class Loan
    {
        [Key]
        public int LoanId { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public Book Book { get; set; }

        [ForeignKey("Borrower")]
        public int BorrowerId { get; set; }
        public Borrowers Borrower { get; set; }

        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool IsReturned { get; set; }
    }
}