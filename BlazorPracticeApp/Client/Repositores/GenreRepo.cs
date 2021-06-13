using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorPracticeApp.Client.Helpers;
using BlazorPracticeApp.Shared.Entities;
using ShareMovieProto;

namespace BlazorPracticeApp.Client.Repositores
{
    public class GenreRepo : IGenreRepo
    {
        private readonly IHttpService httpService;
        //private readonly string url = "api/Genre";
        private readonly GenreProtoService.GenreProtoServiceClient _genreProtoService;


        public GenreRepo(IHttpService httpService, GenreProtoService.GenreProtoServiceClient genreProtoService)
        {
            this.httpService = httpService;
            _genreProtoService = genreProtoService;
        }

        public async Task Create(Genre genre)
        {
            //var response = await httpService.Post(url, genre);

            //if (!response.IsSuccess)
            //{
            //    throw new ApplicationException(await response.GetBody());
            //}
            GenreSendRequest genreSendRequest = new GenreSendRequest();
            GenreModle genreModel = new GenreModle(){
                Id = genre.Id,
                Name = genre.Name,
            };
            genreSendRequest.GenreM = genreModel;

            await _genreProtoService.InsertGenreAsync(genreSendRequest);
        }

        public async Task UpdateGenre(Genre genre)
        {
            //var response = await httpService.Put(url, genre);

            //if (!response.IsSuccess)
            //{
            //    throw new ApplicationException(await response.GetBody());
            //}
            GenreModle genreModel = new GenreModle()
            {
                Id = genre.Id,
                Name = genre.Name,
            };

            GenreEditRequest request = new GenreEditRequest {GenreM = genreModel};
            await _genreProtoService.EditGenreAsync(request);
        }

        //public async Task<List<Genre>> GetGenre()
        //{
        //    var result = await httpService.Get<List<Genre>>(url);
        //    if (!result.IsSuccess)
        //    {
        //        throw new ApplicationException(await result.GetBody());
        //    }

        //    return result.Response;
        //}
        public async Task<List<Genre>> GetGenre()
        {
            //var response = await httpService.Get<List<Genre>>(url);

            //if (!response.IsSuccess)
            //{
            //    throw new ApplicationException(await response.GetBody());
            //}

            //return response.Response;

            List<Genre> genres = new List<Genre>();

            var request = new GenreRequest();
            var reply = await _genreProtoService.GetAllGenreAsync(request);
            foreach (var item in reply.GenrePakage)
            {
                genres.Add(new Genre()
                {
                    Id = item.Id,
                    Name = item.Name,
                });
            }

            return genres;
        }

        public async Task<Genre> GetGenreById(int id)
        {
            //var response = await httpService.Get<Genre>($"{url}/{id}");

            //if (!response.IsSuccess)
            //{
            //    throw new ApplicationException(await response.GetBody());
            //}

            //return response.Response;

            var request = new GetGenreByIdRequest {Id = id};

            var response = await _genreProtoService.GetGenreByIdAsync(request);
            Genre genre = new Genre()
            {
                Id = response.GenreM.Id,
                Name = response.GenreM.Name,
            };

            return genre;
        }

        public async Task Delete(int id)
        {
            var request = new GenreDeleteRequest(){Id = id};
            var reply =await _genreProtoService.DeleteGenreAsync(request);
            if (reply.Deleted)
            {
                Console.WriteLine("Genre Deleted");
            }
            else
            {
                Console.WriteLine("Delete Fail");
            }

        }
    }
}
