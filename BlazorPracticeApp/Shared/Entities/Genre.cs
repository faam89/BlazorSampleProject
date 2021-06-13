using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlazorPracticeApp.Shared.Entities
{
   public class Genre
    {
        public int Id { get; set; }

        [Display(Name ="نام ژانر")]
        [Required(ErrorMessage ="{0} را وارد کنید")]
        public string Name { get; set; }

        public List<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
    }
}
