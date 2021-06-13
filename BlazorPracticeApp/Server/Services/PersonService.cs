using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorPracticeApp.Server.DbContext;
using BlazorPracticeApp.Server.Utilities;
using BlazorPracticeApp.Shared.Entities;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ShareMovieProto;
//using Google.Protobuf.WellKnownTypes;

namespace BlazorPracticeApp.Server.Services
{
    public class PersonService : PersonProtoService.PersonProtoServiceBase
    {
        private readonly BlazorPracticeDbContext _context;

        public PersonService(BlazorPracticeDbContext context)
        {
            _context = context;
        }

        public override async Task<PersonSendReply> InsertPerson(PersonSendRequest request, ServerCallContext context)
        {

            if (request.PersonModle.Picture != null)
            {
                var image = Convertor.Base64ToImage(request.PersonModle.Picture);
                var picName = Guid.NewGuid().ToString("N") + ".jpeg";
                image.AddImageToServer(picName, PathTools.PeopleImageAddress);
                request.PersonModle.Picture = picName;
            }
            else
                request.PersonModle.Picture = "default.jpg";
            Person person = new Person()
            {
                Biography = request.PersonModle.Biography,
                Birthdate = request.PersonModle.Birthdate.ToDateTime(),
                Name = request.PersonModle.Name,
                Picture = request.PersonModle.Picture,
            };
            if (person.Birthdate != null)
            {
                // Converting value from UTC time zone to make comfortale with iran TimeZone  
                TimeZoneInfo tzObject1 = TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");
                DateTime ut1 = TimeZoneInfo.ConvertTimeFromUtc(person.Birthdate.Value, tzObject1);
                person.Birthdate = ut1;

            }
            try
            {
                PersonSendReply reply = new PersonSendReply();
                await _context.Persons.AddAsync(person);
                await _context.SaveChangesAsync();
                reply.SentId = person.Id;
                return await Task.FromResult(reply);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }

        }

        public override async Task<PersonReplay> GetAllPerson(PersonRequest request, ServerCallContext context)
        {
            List<Person> people = new List<Person>();//قالبی که باید با دیتا بیس پر کنیم
            people = _context.Persons.Include(w => w.MoviesActors).ToList();

            //var personModels = AddPerson(people); // قالبی که با ریپلای برگشت داده میشود 

            //List<PersonModle> personModels = new List<PersonModle>();
            //foreach (var item in people)
            //{

            //    if (item.Birthdate != null)
            //    {

            //        personModels.Add(new PersonModle()
            //        {
            //            Id = item.Id,
            //            Biography = item.Biography,
            //            Birthdate = Timestamp.FromDateTime(TimeZoneInfo.ConvertTimeToUtc(item.Birthdate ?? DateTime.Now)),
            //            //Character = item.MoviesActors.FirstOrDefault()?.Character /*?? "kra"*/,
            //            Name = item.Name,
            //            Picture = PathTools.PeopleShowImageAddress + item.Picture,
            //        });
            //    }
            //}
            var reply = new PersonReplay();
            reply.PersonPakage.AddRange(AddPerson(people));
            return await Task.FromResult(reply);
        }

        public override async Task<FilterPeopleReplay> FilterPeopleByName(FilterPeopleRequest request, ServerCallContext context)
        {
            List<Person> people = new List<Person>();
            people = _context.Persons.Where(w => w.Name.Contains(request.SearchText)).Take(5).ToList();
            
            FilterPeopleReplay reply = new FilterPeopleReplay();
            reply.PersonPakage.AddRange(AddPerson(people));
            return await Task.FromResult(reply);
        }

        public override async Task<GetPersonByIdReply> GetPersonById(GetPersonByIdRequest request, ServerCallContext context)
        {
            //Person person = new Person();

            var person = _context.Persons.SingleOrDefault(w => w.Id.Equals(request.Id));
            if (person != null)
            {
                PersonModle personModel = new PersonModle()
                {
                    Id = person.Id,
                    Biography = person.Biography,
                    Birthdate = Timestamp.FromDateTime(TimeZoneInfo.ConvertTimeToUtc(person.Birthdate ?? DateTime.Now)),
                    //Character = person.Character,
                    Picture = PathTools.PeopleShowImageAddress + person.Picture,
                    Name = person.Name,
                };
                GetPersonByIdReply reply = new GetPersonByIdReply()
                {
                    PersonM = personModel
                };
                return await Task.FromResult(reply);

            }
            else
                return null;
        }

        public override async Task<PersonEditReply> EditPerson(PersonEditRequest request, ServerCallContext context)
        {
            var person = _context.Persons.SingleOrDefault(w => w.Id.Equals(request.PersonM.Id));
            if (person != null)
            {
                if (request.PersonM.Picture != null /*"noPic"*/)
                {
                    var image = Convertor.Base64ToImage(request.PersonM.Picture);
                    var picName = Guid.NewGuid().ToString("N") + ".jpeg";
                    image.AddImageToServer(picName, PathTools.PeopleImageAddress, person.Picture);
                    person.Picture = picName;
                }

                person.Id = request.PersonM.Id;
                person.Name = request.PersonM.Name;
                person.Biography = request.PersonM.Biography;
                person.Birthdate = request.PersonM.Birthdate.ToDateTime();
                //person.Character = request.PersonM.Character;
                _context.Persons.Update(person);
                await _context.SaveChangesAsync();

                PersonEditReply reply = new PersonEditReply();

                return await Task.FromResult(reply);
            }
            else
                return null;
        }

        public override async Task<PersonDeleteReplay> DeletePerson(PersonDeleteRequest request, ServerCallContext context)
        {
            var person = _context.Persons.SingleOrDefault(w => w.Id.Equals(request.Id));
            PersonDeleteReplay reply = new PersonDeleteReplay();
            if (person != null)
            {
                _context.Persons.Remove(person);
                await _context.SaveChangesAsync();
                reply.Deleted = true;
                return await Task.FromResult(reply);
            }
            else
            {
                reply.Deleted = false;
                return await Task.FromResult(reply);
            }
        }

        public List<PersonModle> AddPerson(List<Person> people)
        {
            List<PersonModle> personModels = new List<PersonModle>();
            foreach (var item in people)
            {
                personModels.Add(new PersonModle()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Biography = item.Biography,
                    Birthdate = Timestamp.FromDateTime(TimeZoneInfo.ConvertTimeToUtc(item.Birthdate ?? DateTime.Now)),
                    //Character = item.Character,
                    Picture = PathTools.PeopleShowImageAddress + item.Picture,

                });
            }

            return personModels;
        }

    }
}
