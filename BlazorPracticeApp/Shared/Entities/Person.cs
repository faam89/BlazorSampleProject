using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorPracticeApp.Shared.Entities
{
   public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Biography  { get; set; }

        public string Picture { get; set; }

        [Display(Name = "تاریخ تولد")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public DateTime? Birthdate { get; set; }

        [NotMapped]
        public string Character { get; set; }

        public List<MovieActor> MoviesActors { get; set; } = new List<MovieActor>();
    }
}
