using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorPracticeApp.Server.DbContext;
using BlazorPracticeApp.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorPracticeApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class GenreController : ControllerBase
    {
        private readonly BlazorPracticeDbContext _context;
       
        public GenreController(BlazorPracticeDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Genre genre)
        {
            await _context.AddAsync(genre);
            await _context.SaveChangesAsync();

            return genre.Id;
        }

        [HttpGet]
        public async Task<ActionResult<List<Genre>>> Get()
        {
            return await _context.Genres.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> Get(int id)
        {
            var genre = await _context.Genres.SingleOrDefaultAsync(s => s.Id.Equals(id));
            if (genre == null) return NotFound();
            else
                return genre;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Genre genre)
        {
            _context.Update(genre);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
