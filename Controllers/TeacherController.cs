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
    public class TeachersController : ControllerBase
    {
        private readonly UniversityContext _context;

        public TeachersController(UniversityContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetAllTeachers()
        {
            return await _context.Teachers.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> GetTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();

            return teacher;
        }

        [HttpPost]
        public async Task<ActionResult<Teacher>> CreateTeacher([FromBody] Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
            return Ok(teacher);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeacher(int id, [FromBody] Teacher updatedTeacher)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();

            teacher.FirstName = updatedTeacher.FirstName;
            teacher.LastName = updatedTeacher.LastName;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
