using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorPracticeApp.Server.DbContext;
using BlazorPracticeApp.Server.Utilities;
using BlazorPracticeApp.Shared.DTOs;
using BlazorPracticeApp.Shared.Entities;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ShareMovieProto;

namespace BlazorPracticeApp.Server.Services
{
    public class MovieService : MovieProtoService.MovieProtoServiceBase
    {
        private readonly BlazorPracticeDbContext _context;

        public MovieService(BlazorPracticeDbContext context)
        {
            _context = context;
        }
        public override async Task<MovieSendReply> InsertMovie(MovieSendRequest request, ServerCallContext context)
        {
            if (request.MovieM.Poster != null)
            {
                var image = Convertor.Base64ToImage(request.MovieM.Poster);
                var picName = Guid.NewGuid().ToString("N") + ".jpeg";
                image.AddImageToServer(picName, PathTools.MoviePosterAddress);
                request.MovieM.Poster = picName;
            }
            else
                request.MovieM.Poster = "Default.jpg";

            Movie movie = new Movie()
            {
                Poster = request.MovieM.Poster,
                InTheater = request.MovieM.InTheater,
                Summary = request.MovieM.Summary,
                Title = request.MovieM.Title,
                Trailer = request.MovieM.Trailer,
                ReleaseDate = request.MovieM.ReleaseDate.ToDateTime(),
            };
            if (movie.ReleaseDate != null)
            {
                // Converting value from UTC time zone to make comfortale with iran TimeZone  
                TimeZoneInfo tzObject1 = TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");
                DateTime ut1 = TimeZoneInfo.ConvertTimeFromUtc(movie.ReleaseDate.Value, tzObject1);
                movie.ReleaseDate = ut1;
            }
            foreach (var item in request.MovieM.MovieGenreM)
            {
                movie.MovieGenres.Add(new MovieGenre()
                {
                    GenreId = item.GenreId,
                    MovieId = item.MovieId,
                });
            }
            foreach (var item in request.MovieM.MovieActorM)
            {
                movie.MovieActors.Add(new MovieActor()
                {
                    MovieId = item.MovieId,
                    Character = item.Character,
                    Order = item.Order,
                    PersonId = item.PersonId,
                });
            }

            MovieSendReply reply = new MovieSendReply();
            try
            {
                await _context.Movies.AddAsync(movie);
                await _context.SaveChangesAsync();

                reply.Id = movie.Id;
                return await Task.FromResult(reply);
            }
            catch (Exception e)
            {
                reply.Id = movie.Id;
                Console.WriteLine(e.ToString());
                return await Task.FromResult(reply);
            }
        }

        public override async Task<MovievDetailDTOReply> GetDetailMovieDTO(MovieDetailDTORequest request,
            ServerCallContext context)
        {
            var movie = await _context.Movies
                .Include(s => s.MovieActors).ThenInclude(s => s.Person)
                .Include(s => s.MovieGenres).ThenInclude(s => s.Genre)
                .SingleOrDefaultAsync(s => s.Id == request.Id);

            if (movie == null) return null;
            //var model = new DetailMovieDTO { MovieDto = movie };
            //model.Actors = movie.MovieActors.Select(s => new Person
            //{
            //    Id = s.PersonId,
            //    Character = s.Character,
            //    Name = s.Person.Name,
            //    Picture = PathTools.PeopleShowImageAddress + s.Person.Picture
            //}).ToList();
            //model.Genres = movie.MovieGenres.Select(s => s.Genre).ToList();
            var reply = new MovievDetailDTOReply();
            reply.DetailMovie = new DetailMovieModel();

            reply.DetailMovie.Id = movie.Id;
            reply.DetailMovie.InTheater = movie.InTheater;
            reply.DetailMovie.Poster = PathTools.MoviePosterShowAddress + movie.Poster;
            reply.DetailMovie.Summary = movie.Summary;
            reply.DetailMovie.ReleaseDate = Timestamp.FromDateTime(TimeZoneInfo.ConvertTimeToUtc(movie.ReleaseDate ?? DateTime.Now));
            reply.DetailMovie.Title = movie.Title;
            reply.DetailMovie.Trailer = movie.Trailer;

            if (movie.MovieActors.Count > 0)
            {
                foreach (var item in movie.MovieActors)
                {
                    reply.DetailMovie.PersonM.Add(new DetailMovieModel.Types.PersonMo()
                    {
                        Id = item.Person.Id,
                        //Biography = item.Person.Biography,
                        //Birthdate = Timestamp.FromDateTime(TimeZoneInfo.ConvertTimeToUtc(item.Person.Birthdate ?? DateTime.Now)),
                        Name = item.Person.Name,
                        Picture = PathTools.PeopleShowImageAddress + item.Person.Picture,
                        Character = item.Character,
                    });
                }
            }

            if (movie.MovieGenres.Count > 0)
            {
                foreach (var item in movie.MovieGenres)
                {
                    reply.DetailMovie.GenreM.Add(new DetailMovieModel.Types.GenreMo()
                    {
                        Id = item.Genre.Id,
                        Name = item.Genre.Name,
                    });
                }
            }
            //reply.DetailMovieDTOModel.PersonM.AddRange(personMlist);
            return await Task.FromResult(reply);


        }

        public override async Task<MovieGetIPIReply> GetIndexPageInfo(MovieGetIPIRequest request, ServerCallContext context)
        {
            // return base.GetIndexPageInfo(request, context);
            var limit = 6;
            var moviesInTheater = await _context.Movies
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

            var upcomingReleases = await _context.Movies
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
            var reply = new MovieGetIPIReply { IndexPageDTOModel = new IndexPageDTOProto() };

            if (moviesInTheater.Count > 0)
            {
                foreach (var item in moviesInTheater)
                {

                    reply.IndexPageDTOModel.InTheaters.Add(new MovieModel()
                    {
                        Id = item.Id,
                        InTheater = item.InTheater,
                        Poster = item.Poster,
                        Summary = item.Summary,
                        Title = item.Title,
                        Trailer = item.Trailer,
                        ReleaseDate = Timestamp.FromDateTime(TimeZoneInfo.ConvertTimeToUtc(item.ReleaseDate ?? DateTime.Now)),
                        MovieGenreM = {item.MovieGenres.Select(s=>new MovieModel.Types.MovieGenreModel()
                    {
                        GenreId = s.GenreId,
                        MovieId = s.MovieId,
                    })},
                        MovieActorM = {item.MovieActors.Select(s=>new MovieModel.Types.MovieActorModel()
                    {
                        Character = s.Character,
                        MovieId = s.MovieId,
                        Order = s.Order,
                        PersonId = s.PersonId
                    })},
                    });
                }
            }
            if (upcomingReleases.Count > 0)
            {
                foreach (var item in upcomingReleases)
                {

                    reply.IndexPageDTOModel.UpcomingReleases.Add(new MovieModel()
                    {
                        Id = item.Id,
                        InTheater = item.InTheater,
                        Poster = item.Poster,
                        Summary = item.Summary,
                        Title = item.Title,
                        Trailer = item.Trailer,
                        ReleaseDate = Timestamp.FromDateTime(TimeZoneInfo.ConvertTimeToUtc(item.ReleaseDate ?? DateTime.Now)),
                        MovieGenreM = {item.MovieGenres.Select(s=>new MovieModel.Types.MovieGenreModel()
                        {
                            GenreId = s.GenreId,
                            MovieId = s.MovieId,
                        })},
                        MovieActorM = {item.MovieActors.Select(s=>new MovieModel.Types.MovieActorModel()
                    {
                        Character = s.Character,
                        MovieId = s.MovieId,
                        Order = s.Order,
                        PersonId = s.PersonId
                    })},
                    });
                }
            }
            return await Task.FromResult(reply);
        }

        public override async Task<MovievReply> GetMovieForFilter(MovieRequest request, ServerCallContext context)
        {
            List<Movie> movieList;
            List<Movie> enumerable;
            if (request.InTheaters.Equals(false) && request.GenreId.Equals(0) && request.Title.Equals(""))
            {
                enumerable = await _context.Movies.Where(w=>w.InTheater||!w.InTheater)
                    .Include(w => w.MovieGenres)
                    .Include(ws => ws.MovieActors)
                    .ToListAsync();
                if (enumerable == null) return null;

            }
            else
            {
                movieList = await _context.Movies
                    .Include(w => w.MovieGenres).ThenInclude(w => w.Genre)
                    .Include(ws => ws.MovieActors).ThenInclude(w => w.Person).ToListAsync();
                if (movieList == null) return null;
                enumerable = movieList.Where(s => s.Title.Contains(request.Title) &&
                                                     s.InTheater.Equals(request.InTheaters) &&
                                                     s.MovieGenres.Exists(w => w.GenreId.Equals(request.GenreId))).ToList();
            }
            //var request = new MovieRequest();
            var reply = new MovievReply();
            foreach (var item in enumerable)
            {
                reply.MoviePakage.Add(new MovieModel()
                {
                    Id = item.Id,
                    Title = item.Title,
                    InTheater = item.InTheater,
                    Poster = PathTools.MoviePosterShowAddress + item.Poster,
                    ReleaseDate = Timestamp.FromDateTime(TimeZoneInfo.ConvertTimeToUtc(item.ReleaseDate ?? DateTime.Now)),
                    Summary = item.Summary,
                    Trailer = item.Trailer,
                    MovieGenreM = {item.MovieGenres.Select(s=>new MovieModel.Types.MovieGenreModel()
                    {
                        GenreId = s.GenreId,
                        MovieId = s.MovieId,
                    })},
                    MovieActorM = {item.MovieActors.Select(s=>new MovieModel.Types.MovieActorModel()
                    {
                        Character = s.Character,
                        MovieId = s.MovieId,
                        Order = s.Order,
                        PersonId = s.PersonId
                    })},
                });
            }

            return await Task.FromResult(reply);
        }
    }
}
