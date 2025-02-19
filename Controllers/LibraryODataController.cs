using InmindLab3LibraryDB.Data;
using InmindLab3LibraryDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using System.Linq;

namespace InmindLab3LibraryDB.Controllers
{
    [Route("odata/[controller]")]
    public class LibraryODataController : ODataController
    {
        private readonly LibraryContext _context;

        public LibraryODataController(LibraryContext context)
        {
            _context = context;
        }

        [EnableQuery]
        [HttpGet("Books")]
        public IActionResult GetBooks()
        {
            return Ok(_context.Books);
        }

        [HttpGet("BooksByYear/{year}")]
        public IActionResult GetBooksByYear(int year)
        {
            var books = _context.Books.Where(b => b.PublishedYear == year);
            return Ok(books);
        }

        [HttpGet("GroupAuthorsByBirthYear")]
        public IActionResult GroupAuthorsByBirthYear()
        {
            var groups = _context.Authors
                .GroupBy(a => a.BirthDate.Year)
                .Select(g => new 
                { 
                    BirthYear = g.Key, 
                    Authors = g.ToList() 
                });
            return Ok(groups);
        }

        [HttpGet("GroupAuthorsByYearAndCountry")]
        public IActionResult GroupAuthorsByYearAndCountry()
        {
            var groups = _context.Authors
                .GroupBy(a => new { a.BirthDate.Year, a.Country })
                .Select(g => new 
                { 
                    g.Key.Year, 
                    g.Key.Country, 
                    Authors = g.ToList() 
                });
            return Ok(groups);
        }

        [HttpGet("TotalBooks")]
        public IActionResult GetTotalBooks()
        {
            var total = _context.Books.Count();
            return Ok(total);
        }

        [HttpGet("PaginatedBooks")]
        public IActionResult GetPaginatedBooks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var books = _context.Books
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
            return Ok(books);
        }
    }
}
