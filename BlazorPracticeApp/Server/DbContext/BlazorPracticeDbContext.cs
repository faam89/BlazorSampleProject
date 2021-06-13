using BlazorPracticeApp.Shared.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorPracticeApp.Server.DbContext
{
    public class BlazorPracticeDbContext : IdentityDbContext
    {
        public BlazorPracticeDbContext(DbContextOptions<BlazorPracticeDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieGenre>().HasKey(s => new { s.MovieId, s.GenreId });
            modelBuilder.Entity<MovieActor>().HasKey(s => new { s.MovieId, s.PersonId });

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Genre> Genres { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<MovieGenre> MovieGenres { get; set; }

        public DbSet<MovieActor> MoviesActors { get; set; }
    }
}