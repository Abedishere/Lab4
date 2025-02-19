using System.ComponentModel.DataAnnotations;

namespace InmindLab3LibraryDB.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Country { get; set; }
    }
}