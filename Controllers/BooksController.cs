using Microsoft.AspNetCore.Mvc;
using InmindLab3LibraryDB.Data;
using InmindLab3LibraryDB.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InmindLab3LibraryDB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet("by-year/{year}")]
        public IActionResult GetBooksByYear(int year, [FromQuery] string sortOrder = "asc")
        {
            var query = _context.Books
                .Where(b => b.PublishedYear == year);

            query = sortOrder.ToLower() == "desc" 
                ? query.OrderByDescending(b => b.PublishedYear) 
                : query.OrderBy(b => b.PublishedYear);

            return Ok(query.ToList());
        }

        [HttpGet("authors/by-birth-year")]
        public IActionResult GroupAuthorsByBirthYear()
        {
            var groups = _context.Authors
                .GroupBy(a => a.BirthDate.Year)
                .Select(g => new { BirthYear = g.Key, Authors = g.ToList() });
            return Ok(groups);
        }

        [HttpGet("authors/by-year-country")]
        public IActionResult GroupAuthorsByYearAndCountry()
        {
            var groups = _context.Authors
                .GroupBy(a => new { a.BirthDate.Year, a.Country })
                .Select(g => new { 
                    g.Key.Year, 
                    g.Key.Country, 
                    Authors = g.ToList() 
                });
            return Ok(groups);
        }

        [HttpGet("total")]
        public IActionResult GetTotalBooks() => Ok(_context.Books.Count());

        [HttpGet("paginated")]
        public IActionResult GetPaginatedBooks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
            => Ok(_context.Books
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList());
    }
}
