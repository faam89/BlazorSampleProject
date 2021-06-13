using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BlazorPracticeApp.Server.DbContext;
using BlazorPracticeApp.Shared.Entities;
using Grpc;
using Grpc.Core;
using ShareMovieProto;


namespace BlazorPracticeApp.Server.Services
{
    public class GenreService : GenreProtoService.GenreProtoServiceBase
    {
        private readonly BlazorPracticeDbContext _context;

        public GenreService(BlazorPracticeDbContext context)
        {
            _context = context;
        }

        public override async Task<GenreSendReply> InsertGenre(GenreSendRequest genreModel, ServerCallContext context)
        {
            GenreSendReply sendReply = new GenreSendReply();

            Genre genre = new Genre()
            {
                Id = genreModel.GenreM.Id,
                Name = genreModel.GenreM.Name,
            };

            try
            {
                await _context.Genres.AddAsync(genre);
                if (await _context.SaveChangesAsync() == 1)
                {
                    sendReply.Sent = true;
                    Console.WriteLine("Done");
                    return await Task.FromResult(sendReply);
                }
                else
                {
                    sendReply.Sent = false;
                    Console.WriteLine("Cant Save ");
                    return await Task.FromResult(sendReply);
                }
            }
            catch (Exception e)
            {
                sendReply.Sent = false;
                Console.WriteLine(e.ToString());
                return await Task.FromResult(sendReply);
            }
        }

        public override async Task<GenreReplay> GetAllGenre(GenreRequest genreRequest, ServerCallContext context)
        {
            List<Genre> genres = new List<Genre>();
            genres = _context.Genres.ToList();

            List<GenreModle> genreModels = new List<GenreModle>();

            foreach (var item in genres)
            {
                genreModels.Add(new GenreModle()
                {
                    Id = item.Id,
                    Name = item.Name,
                });
            }

            var response = new GenreReplay();
            response.GenrePakage.AddRange(genreModels);
            return await Task.FromResult(response);
        }

        public override async Task<GetGenreByIdReply> GetGenreById(GetGenreByIdRequest request, ServerCallContext context)
        {
            GetGenreByIdReply reply = new GetGenreByIdReply();
            var genre = _context.Genres.Single(w => w.Id.Equals(request.Id));
            GenreModle genreModel = new GenreModle()
            {
                Id = genre.Id,
                Name = genre.Name,
            };
            reply.GenreM = genreModel;
            return await Task.FromResult(reply);
        }

        public override async Task<GenreDeleteReplay> DeleteGenre(GenreDeleteRequest request, ServerCallContext context)
        {
            GenreDeleteReplay replay = new GenreDeleteReplay();
            var genre = _context.Genres.SingleOrDefault(w => w.Id.Equals(request.Id));
            if (genre != null)
            {
                try
                {
                    _context.Genres.Remove(genre);
                   await _context.SaveChangesAsync();
                    replay.Deleted = true;
                    return await Task.FromResult(replay);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    replay.Deleted = false;
                    return await Task.FromResult(replay);
                }
            }
            else
            {
                replay.Deleted = true;
                return await Task.FromResult(replay);
            }
        }

        public override async Task<GenreEditReply> EditGenre(GenreEditRequest request, ServerCallContext context)
        {
            var genre = _context.Genres.SingleOrDefault(w => w.Id.Equals(request.GenreM.Id));

            if (genre != null)
            {
                genre.Name = request.GenreM.Name;
                _context.Genres.Update(genre);
                await _context.SaveChangesAsync();
            }

            GenreEditReply reply = new GenreEditReply();
            return await Task.FromResult(reply);
        }
    }
}
