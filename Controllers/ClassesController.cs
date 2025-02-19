using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InmindLab3_4part2.Data;
using InmindLab3_4part2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InmindLab3_4part2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassesController : ControllerBase
    {
        private readonly UniversityContext _context;

        public ClassesController(UniversityContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> GetAllClasses()
        {
            return await _context.Classes
                .Include(c => c.Course)
                .Include(c => c.Teacher)
                .Include(c => c.Students)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Class>> GetClass(int id)
        {
            var classEntity = await _context.Classes
                .Include(c => c.Course)
                .Include(c => c.Teacher)
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classEntity == null) return NotFound();
            return classEntity;
        }

        [HttpPost]
        public async Task<ActionResult<Class>> CreateClass([FromBody] Class classEntity)
        {
            _context.Classes.Add(classEntity);
            await _context.SaveChangesAsync();
            return Ok(classEntity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClass(int id, [FromBody] Class updatedClass)
        {
            var classEntity = await _context.Classes.FindAsync(id);
            if (classEntity == null) return NotFound();

            classEntity.CourseId = updatedClass.CourseId;
            classEntity.TeacherId = updatedClass.TeacherId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            var classEntity = await _context.Classes.FindAsync(id);
            if (classEntity == null) return NotFound();

            _context.Classes.Remove(classEntity);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("{classId}/enroll/{studentId}")]
        public async Task<IActionResult> EnrollStudent(int classId, int studentId)
        {
            var classEntity = await _context.Classes
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == classId);

            if (classEntity == null) return NotFound("Class not found.");

            var student = await _context.Students.FindAsync(studentId);
            if (student == null) return NotFound("Student not found.");

            classEntity.Students.Add(student);
            await _context.SaveChangesAsync();

            return Ok("Student enrolled.");
        }

        [HttpDelete("{classId}/unenroll/{studentId}")]
        public async Task<IActionResult> UnenrollStudent(int classId, int studentId)
        {
            var classEntity = await _context.Classes
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == classId);

            if (classEntity == null) return NotFound("Class not found.");

            var student = await _context.Students.FindAsync(studentId);
            if (student == null) return NotFound("Student not found.");

            classEntity.Students.Remove(student);
            await _context.SaveChangesAsync();

            return Ok("Student unenrolled.");
        }
    }
}
