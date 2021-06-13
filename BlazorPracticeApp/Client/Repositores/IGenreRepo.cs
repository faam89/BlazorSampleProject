using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorPracticeApp.Shared.Entities;

namespace BlazorPracticeApp.Client.Repositores
{
    public interface IGenreRepo
    {
        Task Create(Genre genre);


        Task UpdateGenre(Genre genre);

        Task<List<Genre>> GetGenre();

        Task<Genre> GetGenreById(int id);

        Task Delete(int id);
    }
}
