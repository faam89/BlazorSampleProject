using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorPracticeApp.Client.Helpers;
using BlazorPracticeApp.Shared.DTOs;
using BlazorPracticeApp.Shared.Entities;
using Google.Protobuf.WellKnownTypes;
using ShareMovieProto;


namespace BlazorPracticeApp.Client.Repositores
{
    public class MovieRepo : IMovieRepo
    {
        private readonly IHttpService _httpService;
       // private readonly string url = "api/Movies";
        private readonly MovieProtoService.MovieProtoServiceClient _movieProtoService;

        public MovieRepo(IHttpService httpService, MovieProtoService.MovieProtoServiceClient movieProtoService)
        {
            _httpService = httpService;
            _movieProtoService = movieProtoService;

        }

        public async Task<int> Create(Movie movie)
        {
            //var response = await _httpService.Post<Movie, int>(url, movie);
            //if (!response.IsSuccess)
            //{
            //    throw new ApplicationException(await response.GetBody());
            //}

            //return response.Response;

            var movieModel = new MovieModel()
            {
                InTheater = movie.InTheater,
                Poster = movie.Poster,
                ReleaseDate = Timestamp.FromDateTime(TimeZoneInfo.ConvertTimeToUtc(movie.ReleaseDate ?? DateTime.Now)),
                Summary = movie.Summary,
                Title = movie.Title,
                Trailer = movie.Trailer,
            };

            foreach (var item in movie.MovieGenres)
            {
                movieModel.MovieGenreM.Add(new MovieModel.Types.MovieGenreModel()
                {
                    GenreId = item.GenreId,
                    MovieId = item.MovieId,
                });
            }
            foreach (var item in movie.MovieActors)
            {
                movieModel.MovieActorM.Add(new MovieModel.Types.MovieActorModel()
                {
                    MovieId = item.MovieId,
                    Character = item.Character,
                    Order = item.Order,
                    PersonId = item.PersonId,
                });
            }
            var request = new MovieSendRequest()
            {
                MovieM = movieModel
            };
            var reply = await _movieProtoService.InsertMovieAsync(request);
            return reply.Id;
        }

        public async Task<List<Movie>> GetAll(string title, bool inTheaters, int genreId)
        {
            MovieRequest request = new MovieRequest 
                {
                    Title = title,
                    GenreId = genreId,
                    InTheaters = inTheaters, 
                };
            var reply = new MovievReply();
            reply = await _movieProtoService.GetMovieForFilterAsync(request);

            var movieList = new List<Movie>();
            foreach (var item in reply.MoviePakage)
            {
                movieList.Add(new Movie()
                {
                    Id = item.Id,
                    Trailer = item.Trailer,
                    Title = item.Title,
                    InTheater = item.InTheater,
                    Poster = item.Poster,
                    Summary = item.Summary,
                    ReleaseDate = item.ReleaseDate.ToDateTime(),
                    MovieActors = item.MovieActorM.Select(w => new MovieActor()
                    {
                        Order = w.Order,
                        Character = w.Character,
                        MovieId = w.MovieId,
                        PersonId = w.PersonId,
                    }).ToList(),
                    MovieGenres = item.MovieGenreM.Select(w => new MovieGenre()
                    {
                        MovieId = w.MovieId,
                        GenreId = w.GenreId,
                    }).ToList(),
                });
            }

            return movieList;
        }

        public async Task<IndexPageDTO> GetIndexPageDto()
        {
            // return await _httpService.GetHelper<IndexPageDTO>(url);
            IndexPageDTO indexPageDto = new IndexPageDTO();
            var request = new MovieGetIPIRequest();

            var reply = new MovieGetIPIReply() { IndexPageDTOModel = new IndexPageDTOProto() };
            reply = await _movieProtoService.GetIndexPageInfoAsync(request);

            foreach (var item in reply.IndexPageDTOModel.InTheaters)
            {
                indexPageDto.InTheaters.Add(new Movie()
                {
                    Id = item.Id,
                    Poster = item.Poster,
                    InTheater = item.InTheater,
                    ReleaseDate = item.ReleaseDate.ToDateTime(),
                    Summary = item.Summary,
                    Title = item.Title,
                    Trailer = item.Trailer,
                    MovieActors = item.MovieActorM.Select(w => new MovieActor()
                    {
                        Order = w.Order,
                        Character = w.Character,
                        MovieId = w.MovieId,
                        PersonId = w.PersonId,
                    }).ToList(),
                    MovieGenres = item.MovieGenreM.Select(w => new MovieGenre()
                    {
                        MovieId = w.MovieId,
                        GenreId = w.GenreId,
                    }).ToList(),
                });
            }

            foreach (var item in reply.IndexPageDTOModel.UpcomingReleases)
            {
                indexPageDto.UpcomingReleases.Add(new Movie()
                {
                    Id = item.Id,
                    Poster = item.Poster,
                    InTheater = item.InTheater,
                    ReleaseDate = item.ReleaseDate.ToDateTime(),
                    Summary = item.Summary,
                    Title = item.Title,
                    Trailer = item.Trailer,
                    MovieActors = item.MovieActorM.Select(w => new MovieActor()
                    {
                        Order = w.Order,
                        Character = w.Character,
                        MovieId = w.MovieId,
                        PersonId = w.PersonId,
                    }).ToList(),
                    MovieGenres = item.MovieGenreM.Select(w => new MovieGenre()
                    {
                        MovieId = w.MovieId,
                        GenreId = w.GenreId,
                    }).ToList(),
                });
            }

            return indexPageDto;
        }


        public async Task<DetailMovieDTO> GetDetailMovieDTO(int id)
        {
            // return await _httpService.GetHelper<DetailMovieDTO>($"{url}/{id}");

            DetailMovieDTO detailMovieDto = new DetailMovieDTO();
            //detailMovieDto.MovieDto = new Movie();
            //detailMovieDto.Genres = new List<Genre>();
            //detailMovieDto.Actors = new List<Person>();

            var request = new MovieDetailDTORequest() { Id = id };
            var reply = await _movieProtoService.GetDetailMovieDTOAsync(request);

            detailMovieDto.MovieDto.Id = reply.DetailMovie.Id;
            detailMovieDto.MovieDto.InTheater = reply.DetailMovie.InTheater;
            detailMovieDto.MovieDto.Poster = reply.DetailMovie.Poster;
            detailMovieDto.MovieDto.ReleaseDate = reply.DetailMovie.ReleaseDate.ToDateTime();
            detailMovieDto.MovieDto.Summary = reply.DetailMovie.Summary;
            detailMovieDto.MovieDto.Title = reply.DetailMovie.Title;
            detailMovieDto.MovieDto.Trailer = reply.DetailMovie.Trailer;

            foreach (var item in reply.DetailMovie.GenreM)
            {
                detailMovieDto.Genres.Add(new Genre()
                {
                    Id = item.Id,
                    Name = item.Name,
                });
                Console.WriteLine(item.Name);
            }

            foreach (var item in reply.DetailMovie.PersonM)
            {
                detailMovieDto.Actors.Add(new Person()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Character = item.Character,
                    Picture = item.Picture,
                });
            }

            Console.WriteLine(detailMovieDto.Genres.Count);

            return detailMovieDto;
        }

        //public async Task<T> Get<T>(string url)
        //{
        //    var response = await _httpService.Get<T>(url);
        //    if (!response.IsSuccess)
        //    {
        //        throw new ApplicationException(await response.GetBody());
        //    }

        //    return response.Response;
        //}
    }
}