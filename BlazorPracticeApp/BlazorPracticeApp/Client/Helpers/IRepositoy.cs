using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorPracticeApp.Shared.Entities;

namespace BlazorPracticeApp.Client.Helpers
{
    public interface IRepositoy
    { 
        List<Movie> GetMovie();
    }
}
