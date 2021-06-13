using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorPracticeApp.Shared.Entities;

namespace BlazorPracticeApp.Client.Helpers
{
    public class Repository : IRepository
    {
        public List<Movie> GetMovie()
        {
            //List<Movie> _movies = new List<Movie>()
            //{
            //    new Movie {Title = "Movie1",ReleaseDate =new DateTime(1390,10,01)},
            //    new Movie {Title = "Movie2",ReleaseDate =new DateTime(1391,12,17)},
            //    new Movie {Title = "Movie3",ReleaseDate =new DateTime(1383,05,01)},
            //    new Movie {Title = "Movie4",ReleaseDate =new DateTime(1399,01,10)}
            //};
            //return _movies;
            return new List<Movie>
            {
                new Movie {Id = 1,Title = "آموزش Blazor", ReleaseDate = new DateTime(2020, 01, 02),Poster = "https://toplearn.com/img/course/img-course-%D8%B3%D9%87-%D8%B4%D9%86%D8%A8%D9%87-%DB%B1%DB%B9-%D9%81%D8%B1%D9%88%D8%B1%D8%AF%DB%8C%D9%86-%DB%B1%DB%B3%DB%B9%DB%B9-30506704-394.jpg"},
                new Movie {Id = 2,Title = "آموزش Flask", ReleaseDate = new DateTime(2010, 10, 16),Poster = "https://toplearn.com/img/course/img-course-%D9%BE%D9%86%D8%AC-%D8%B4%D9%86%D8%A8%D9%87-%DB%B4-%D8%A7%D8%B1%D8%AF%DB%8C%D8%A8%D9%87%D8%B4%D8%AA-%DB%B1%DB%B3%DB%B9%DB%B9-77214426-890.jpg"},
                new Movie {Id = 3,Title = "آموزش انیمیشن سازی", ReleaseDate = new DateTime(2019, 05, 16),Poster = "https://toplearn.com/img/course/img-course-%DB%8C%DA%A9-%D8%B4%D9%86%D8%A8%D9%87-%DB%B1%DB%B4-%D8%A7%D8%B1%D8%AF%DB%8C%D8%A8%D9%87%D8%B4%D8%AA-%DB%B1%DB%B3%DB%B9%DB%B9-7077634-772.jpg"},
            };
        }
    }
}
