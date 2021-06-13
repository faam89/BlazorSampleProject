using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorPracticeApp.Shared.Entities;

namespace BlazorPracticeApp.Client.Repositores
{
    public interface IPersonRepo
    {
        Task Create(Person person);

        Task<List<Person>> GetPeople();

        Task<List<Person>> GetPeopleByName(string searchText);

        Task<Person> GetPersonById(int id);

        Task UpdatePerson(Person person);

        Task Delete(int id);
    }
}
