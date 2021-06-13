using System.Collections.Generic;
using BlazorPracticeApp.Shared.Entities;

namespace BlazorPracticeApp.Shared.DTOs
{
    public class DetailMovieDTO
    {
        public Movie MovieDto { get; set; } = new Movie();

        public List<Genre> Genres { get; set; } = new List<Genre>();

        public List<Person> Actors { get; set; } = new List<Person>();
    }
}
