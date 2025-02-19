using System.ComponentModel.DataAnnotations;

namespace InmindLab3LibraryDB.Models
{
    public class Borrowers
    {
        [Key]
        public int borrower_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public long phone { get; set; }
    }
}