using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorPracticeApp.Client.Helpers;
using BlazorPracticeApp.Shared.Entities;
using Google.Protobuf.WellKnownTypes;
using ShareMovieProto;

namespace BlazorPracticeApp.Client.Repositores
{
    public class PersonRepo : IPersonRepo
    {
        private readonly IHttpService _httpService;
        //private readonly string _url = "api/People";
        private readonly PersonProtoService.PersonProtoServiceClient _personProto;

        public PersonRepo(IHttpService httpService, PersonProtoService.PersonProtoServiceClient personProto)
        {
            _httpService = httpService;
            _personProto = personProto;
        }

        public async Task Create(Person person)
        {
            //var response = await _httpService.Post(_url, person);
            //if (!response.IsSuccess)
            //{
            //    throw new ApplicationException(await response.GetBody());
            //}

            PersonModle personModel = new PersonModle()
            {
                Name = person.Name,
                Biography = person.Biography,
                Birthdate = Timestamp.FromDateTime(TimeZoneInfo.ConvertTimeToUtc(person.Birthdate ?? DateTime.Now)),
                //Character = person.Character ?? "تعیین نشده",
                Picture = person.Picture,
            };

            PersonSendRequest request = new PersonSendRequest()
            {
                PersonModle = personModel
            };
            await _personProto.InsertPersonAsync(request);
        }

        public async Task<List<Person>> GetPeople()
        {
            /*var result =*/
            //return await _httpService.GetHelper<List<Person>>(_url);

            List<Person> persons = new List<Person>();
            var request = new PersonRequest();
            var people = await _personProto.GetAllPersonAsync(request);

            foreach (var item in people.PersonPakage)
            {
                persons.Add(new Person()
                {
                    Id = item.Id,
                    Picture = item.Picture,
                    Name = item.Name,
                    Biography = item.Biography,
                    Birthdate = item.Birthdate.ToDateTime(),
                    //Character = item.Character,
                });
            }

            return persons;
            //if (!result.IsSuccess)
            //{
            //    throw new ApplicationException(await result.GetBody());
            //}

            //return result.Response;
        }

        public async Task<List<Person>> GetPeopleByName(string searchText)
        {
            /*var result =*/
            //return await _httpService.GetHelper<List<Person>>($"{_url}/search/{searchText}");

            //if (!result.IsSuccess)
            //{
            //    throw new ApplicationException(await result.GetBody());
            //}

            //return result.Response;

            var request = new FilterPeopleRequest() { SearchText = searchText };
            var reply = await _personProto.FilterPeopleByNameAsync(request);

            List<Person> persons = new List<Person>();

            foreach (var item in reply.PersonPakage)
            {
                persons.Add(new Person()
                {
                    Id = item.Id,
                    Picture = item.Picture,
                    Name = item.Name,
                    Biography = item.Biography,
                    Birthdate = item.Birthdate.ToDateTime(),
                    //Character = item.Character,
                });
            }

            return persons;
        }

        public async Task<Person> GetPersonById(int id)
        {
            //return await _httpService.GetHelper<Person>($"{_url}/{id}");
            var request = new GetPersonByIdRequest() { Id = id };
            var reply = await _personProto.GetPersonByIdAsync(request);


            Person person = new Person()
            {
                Id = reply.PersonM.Id,
                Picture = reply.PersonM.Picture,
                Name = reply.PersonM.Name,
                Biography = reply.PersonM.Biography,
                Birthdate = reply.PersonM.Birthdate.ToDateTime(),
            };
            return person;
        }

        public async Task UpdatePerson(Person person)
        {
            //var response = await _httpService.Put(_url, person);

            //if (!response.IsSuccess)
            //{
            //    throw new ApplicationException(await response.GetBody());
            //}
            Console.WriteLine(person.Birthdate.ToString());
            Console.WriteLine(person.Biography);
            Console.WriteLine(person.Id);
            Console.WriteLine(person.Name);
            Console.WriteLine(person.Picture);
            PersonModle personModel = new PersonModle()
            {
                Id = person.Id,
                Name = person.Name,
                Birthdate = Timestamp.FromDateTime(TimeZoneInfo.ConvertTimeToUtc(person.Birthdate ?? DateTime.Now)),
                Picture = person.Picture /*?? "noPic"*/,
                Biography = person.Biography,
            };
            var request = new PersonEditRequest() { PersonM = personModel };
            await _personProto.EditPersonAsync(request);
        }

        public async Task Delete(int id)
        {
            var request = new PersonDeleteRequest() { Id = id };
            var reply = await _personProto.DeletePersonAsync(request);

            Console.WriteLine(reply.Deleted ? "Ok" : "Nok");
        }
    }
}
