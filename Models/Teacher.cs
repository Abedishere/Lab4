using System.Collections.Generic;

namespace InmindLab3_4part2.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Navigation
        public ICollection<Class> Classes { get; set; }
    }
}