using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorPracticeApp.Shared.DTOs;
using BlazorPracticeApp.Shared.Entities;

namespace BlazorPracticeApp.Client.Repositores
{
    public interface IMovieRepo
    {
        Task<int> Create(Movie movie);
        Task<List<Movie>> GetAll(string title , bool inTheaters , int genreId);
        Task<IndexPageDTO> GetIndexPageDto();
        Task<DetailMovieDTO> GetDetailMovieDTO(int id);


    }
}