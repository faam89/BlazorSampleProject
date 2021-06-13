using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlazorPracticeApp.Server.DbContext;
using BlazorPracticeApp.Server.Utilities;
using BlazorPracticeApp.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorPracticeApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : Controller
    {
        private readonly BlazorPracticeDbContext _context;
        private readonly IMapper _mapper;

        public PeopleController(BlazorPracticeDbContext db, IMapper mapper)
        {
            _context = db;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Person person)
        {
            var image = Convertor.Base64ToImage(person.Picture);
            var picName = Guid.NewGuid().ToString("N") + ".jpeg";
            image.AddImageToServer(picName, PathTools.PeopleImageAddress);
            person.Picture = picName;
            await _context.AddAsync(person);
            await _context.SaveChangesAsync();
            return person.Id;
        }


        [HttpGet]
        public async Task<ActionResult<List<Person>>> Get()
        {
            return await _context.Persons.ToListAsync();
        }

        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<List<Person>>> FilterByName(string searchText)
        {
            return await _context.Persons.Where(s => s.Name.Contains(searchText)).Select(s => new Person
            {
                Id = s.Id,
                Biography = s.Biography,
                Character = s.Character,
                Birthdate = s.Birthdate,
                Name = s.Name,
                Picture = PathTools.PeopleShowImageAddress + s.Picture
            }).Take(5).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> Get(int id)
        {
            var resultPerson = await _context.Persons.SingleOrDefaultAsync(s => s.Id.Equals(id));
            resultPerson.Picture = PathTools.PeopleShowImageAddress + resultPerson.Picture;
            if (resultPerson == null) return NotFound();
            return resultPerson;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Person person)
        {
            var mainPerson = await _context.Persons.SingleOrDefaultAsync(s => s.Id.Equals(person.Id));
            if (mainPerson == null) return NotFound();

            mainPerson = _mapper.Map(person, mainPerson);
          
            if (!string.IsNullOrEmpty(person.Picture))
            {
                var image = Convertor.Base64ToImage(person.Picture);
                var picName = Guid.NewGuid().ToString("N") + ".jpeg";
                image.AddImageToServer(picName, PathTools.PeopleImageAddress, mainPerson.Picture);
                mainPerson.Picture = picName;
            }

            await _context.SaveChangesAsync();
            return NoContent();


        }
    }
}
