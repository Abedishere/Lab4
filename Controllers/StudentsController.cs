using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InmindLab3_4part2.Data;
using InmindLab3_4part2.DTOs;
using InmindLab3_4part2.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InmindLab3_4part2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly UniversityContext _context;
        private readonly IMapper _mapper;

        public StudentsController(UniversityContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto studentDto)
        {
            var student = _mapper.Map<Student>(studentDto);
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            var studentView = _mapper.Map<StudentViewDto>(student);
            return Ok(studentView);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentViewDto>>> GetStudents()
        {
            var students = await _context.Students.ToListAsync();
            var studentViews = _mapper.Map<IEnumerable<StudentViewDto>>(students);
            return Ok(studentViews);
        }
    }
}