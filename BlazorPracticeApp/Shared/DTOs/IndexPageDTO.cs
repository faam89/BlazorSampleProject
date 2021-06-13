using System.Collections.Generic;
using BlazorPracticeApp.Shared.Entities;

namespace BlazorPracticeApp.Shared.DTOs
{
    public class IndexPageDTO
    {
        public List<Movie> InTheaters { get; set; } = new List<Movie>();

        public List<Movie> UpcomingReleases { get; set; } = new List<Movie>();
    }
}
