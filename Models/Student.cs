using System.Collections.Generic;

namespace InmindLab3_4part2.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Navigation (many-to-many)
        public ICollection<Class> Classes { get; set; }
    }
}