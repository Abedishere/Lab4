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
    public class CoursesController : ControllerBase
    {
        private readonly UniversityContext _context;

        public CoursesController(UniversityContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetAllCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            return course;
        }

        [HttpPost]
        public async Task<ActionResult<Course>> CreateCourse([FromBody] Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return Ok(course);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] Course updatedCourse)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            course.Title = updatedCourse.Title;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}