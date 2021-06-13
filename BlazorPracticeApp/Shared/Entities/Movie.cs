using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorPracticeApp.Shared.Entities
{
    public class Movie
    {

        public int Id { get; set; }

        [Display(Name ="عنوان")]
        [Required(ErrorMessage ="{0} را وارد کنید ")]
        public string Title { get; set; }
        public string Summary { get; set; }
        public bool InTheater { get; set; }
        public string Trailer { get; set; }

        [Required]
        public DateTime? ReleaseDate { get; set; }

        public string Poster { get; set; }

        public string TitleBrief 
        {
            

            get
            {
                if (string.IsNullOrEmpty(Title))
                {
                    return null;
                }

                if (Title.Length > 60)
                {
                    return Title.Substring(0, 60) + "...";
                }

                return Title;
            }

        }

        public List<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
        public List<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
    }
}
