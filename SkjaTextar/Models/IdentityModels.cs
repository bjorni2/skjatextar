using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Collections.Generic;

namespace SkjaTextar.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser
    {
        public int Edits { get; set; }
        public int NewTranslations { get; set; }
        public int Score
        {
            get
            {
                return this.Edits + (this.NewTranslations * 5);
            }
        }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<TranslationVote> TranslationVotes { get; set; }
        public virtual ICollection<RequestVote> RequestVotes { get; set; }
        public virtual ICollection<Achievement> Achievements { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
            : base("SQLserverinnokkar")
        {
        }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Clip> Clips { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<Request> Requests { get; set; }
    }

    public class ApplicationInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var achievements = new List<Achievement>
            {
                new Achievement{ ID = 1, AchievementText = "1 lína þýdd" },
                new Achievement{ ID = 2, AchievementText = "5 línur þýddar" },
                new Achievement{ ID = 3, AchievementText = "25 línur þýddar" },
                new Achievement{ ID = 4, AchievementText = "100 línur þýddar" },
                new Achievement{ ID = 5, AchievementText = "1 ný þýðing" },
                new Achievement{ ID = 6, AchievementText = "5 nýjar þýðingar" },
                new Achievement{ ID = 7, AchievementText = "25 nýjar þýðingar" },
                new Achievement{ ID = 8, AchievementText = "100 nýjar þýðingar" }
            };
            achievements.ForEach(a => context.Achievements.Add(a));
            context.SaveChanges();

            var categories = new List<Category>
            {
                new Category{ ID = 1, Name = "Gaman" },
                new Category{ ID = 2, Name = "Spenna" },
                new Category{ ID = 3, Name = "Drama" },
                new Category{ ID = 4, Name = "Barnaefni" }
            };
            categories.ForEach(c => context.Categories.Add(c));
            context.SaveChanges();

            var movies = new List<Movie>
            {
                new Movie{ ID = 1, CategoryID = 2, Title = "Die Hard", ReleaseYear = 1988, Active = true },
                new Movie{ ID = 2, CategoryID = 4, Title = "Lion King", ReleaseYear = 1995, Active = false },
                new Movie{ ID = 3, CategoryID = 3, Title = "Brokeback mountain", ReleaseYear = 2005, Active = false },
            };
            movies.ForEach(m => context.Movies.Add(m));
            context.SaveChanges();

            var shows = new List<Show>
            {
                new Show{ ID = 4, CategoryID = 4, Title = "Svampur Sveinsson", Series = 2, Episode = 13, ReleaseYear = 2009, Active = false },
                new Show{ ID = 5, CategoryID = 4, Title = "Svampur Sveinsson", Series = 2, Episode = 14, ReleaseYear = 2009, Active = false },
                new Show{ ID = 6, CategoryID = 3, Title = "House of cards", Series = 1, Episode = 3, ReleaseYear = 2013, Active = false },
                new Show{ ID = 7, CategoryID = 1, Title = "How i met your mother", Series = 7, Episode = 13, ReleaseYear = 2011, Active = false },
            };
            shows.ForEach(s => context.Shows.Add(s));
            context.SaveChanges();

            var clips = new List<Clip>
            {
                new Clip{ ID = 8, CategoryID = 1, Title = "Nyan cat", ReleaseYear = 2009, Link = "www.youtube.com/watch?v=wZZ7oFKsKzY", Active = false },
                new Clip{ ID = 9, CategoryID = 1, Title = "Charlie bit my finger", ReleaseYear = 2007, Link = "www.youtube.com/watch?v=_OBlgSz8sSM", Active = true }
            };
            clips.ForEach(c => context.Clips.Add(c));
            context.SaveChanges();

            var translations = new List<Translation>
            {
                new Translation{ ID = 1, Language = "Íslenska", MediaID = 9, NumberOfDownloads = 42, Score = 0, Locked = false },
                new Translation{ ID = 2, Language = "Pólska", MediaID = 1, NumberOfDownloads = 3, Score = 1337, Locked = true },
                new Translation{ ID = 3, Language = "Íslenska", MediaID = 1, NumberOfDownloads = 17, Score = 0, Locked = false },
            };
            translations.ForEach(t => context.Translations.Add(t));
            context.SaveChanges();

            var requests = new List<Request>
            {
                new Request{ ID = 1, Language = "Finnska", Score = 32, MediaID = 2 },
                new Request{ ID = 2, Language = "Íslenska", Score = 18, MediaID = 8 },
                new Request{ ID = 3, Language = "Íslenska", Score = 14, MediaID = 7 },
                new Request{ ID = 4, Language = "Enska", Score = 7, MediaID = 7 },
                new Request{ ID = 5, Language = "Íslenska", Score = 2, MediaID = 5 }
            };
            requests.ForEach(r => context.Requests.Add(r));
            context.SaveChanges();
        }
    }
}