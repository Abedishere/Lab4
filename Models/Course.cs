using System.Collections.Generic;

namespace InmindLab3_4part2.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }

        // Navigation
        public ICollection<Class> Classes { get; set; }
    }
}