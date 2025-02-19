using System.Collections.Generic;

namespace InmindLab3_4part2.Models
{
    public class Class
    {
        public int Id { get; set; }
        
        // Foreign keys
        public int CourseId { get; set; }
        public Course Course { get; set; }
        
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        
        // Many-to-many with Students
        public ICollection<Student> Students { get; set; }
    }
}