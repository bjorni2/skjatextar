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
            : base("SubTitleContext")
        {
        }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Category> Categories { get; set; }
    }

    public class ApplicationInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
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
                new Movie{ ID = 2, CategoryID = 4, Title = "Lion King", ReleaseYear = 1995, Active = true },
                new Movie{ ID = 3, CategoryID = 3, Title = "Brokeback mountain", ReleaseYear = 2005, Active = true },
            };
        }
    }
}