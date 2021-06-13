using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorPracticeApp.Server.DbContext;
using BlazorPracticeApp.Server.Utilities;
using BlazorPracticeApp.Shared.DTOs;
using BlazorPracticeApp.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorPracticeApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : Controller
    {
        #region constructor

        private readonly BlazorPracticeDbContext context;

        public MoviesController(BlazorPracticeDbContext DB)
        {
            this.context = DB;
        }

        #endregion

        [HttpPost]
        public async Task<ActionResult<int>> Post(Movie movie)
        {
            if (movie.Poster != null)
            {
                var image = Convertor.Base64ToImage(movie.Poster);
                var imageName = Guid.NewGuid().ToString("N") + ".jpeg";
                image.AddImageToServer(imageName, PathTools.MoviePosterAddress);
                movie.Poster = imageName;
            }
            else
                movie.Poster = "تصویری وارد نشده است ";

            await context.AddAsync(movie);
            await context.SaveChangesAsync();
            return movie.Id;
        }

        [HttpGet]
        public async Task<ActionResult<IndexPageDTO>> Get()
        {
            var limit = 6;

            var moviesInTheater = await context.Movies
                .Where(s => s.InTheater)
                .OrderByDescending(s => s.ReleaseDate)
                .Select(s => new Movie
                {
                    MovieActors = s.MovieActors,
                    ReleaseDate = s.ReleaseDate,
                    InTheater = s.InTheater,
                    Id = s.Id,
                    Poster = PathTools.MoviePosterShowAddress + s.Poster,
                    Summary = s.Summary,
                    MovieGenres = s.MovieGenres,
                    Title = s.Title,
                    Trailer = s.Trailer
                })
                .Take(limit)
                .ToListAsync();

            var upcomingReleases = await context.Movies
                .Where(s => s.ReleaseDate > DateTime.Today)
                .OrderByDescending(s => s.ReleaseDate)
                .Select(s => new Movie
                {
                    MovieActors = s.MovieActors,
                    ReleaseDate = s.ReleaseDate,
                    InTheater = s.InTheater,
                    Id = s.Id,
                    Poster = PathTools.MoviePosterShowAddress + s.Poster,
                    Summary = s.Summary,
                    MovieGenres = s.MovieGenres,
                    Title = s.Title,
                    Trailer = s.Trailer
                })
                .Take(limit)
                .ToListAsync();

            return new IndexPageDTO
            {
                InTheaters = moviesInTheater,
                UpcomingReleases = upcomingReleases
            };
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<DetailMovieDTO>> Get(int id)
        {
            var movie = await context.Movies
                .Include(s => s.MovieActors).ThenInclude(s => s.Person)
                .Include(s => s.MovieGenres).ThenInclude(s => s.Genre)
                .SingleOrDefaultAsync(s => s.Id == id);

            if (movie == null) return NotFound();

            var model = new DetailMovieDTO {MovieDto = movie};
            model.MovieDto.Poster = PathTools.MoviePosterShowAddress + movie.Poster;
            model.Actors = movie.MovieActors.Select(s => new Person
            {
                Id = s.PersonId,
                Character = s.Character,
                Name = s.Person.Name,
                Picture = PathTools.PeopleShowImageAddress + s.Person.Picture
            }).ToList();

            model.Genres = movie.MovieGenres.Select(s => s.Genre).ToList();

            return model;
        }
    }
}
