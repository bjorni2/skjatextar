using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Collections.Generic;
using System;
using Microsoft.AspNet.Identity;

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
            //: base("SubTitleContext")
            //:base("RUSQLServer")
        {
        }

        public DbSet<Language> Languages { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Clip> Clips { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<TranslationSegment> TranslationSegments { get; set; }
        public DbSet<Request> Requests { get; set; }
		public DbSet<Report> Reports { get; set; }
    }
    
   /* public class ApplicationInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var languages = new List<Language>
            {
                new Language{ ID = 1, Name = "íslenska" },
                new Language{ ID = 2, Name = "Deutsch" },
                new Language{ ID = 3, Name = "English" },
                new Language{ ID = 4, Name = "español" },
                new Language{ ID = 5, Name = "français" },
                new Language{ ID = 6, Name = "Nederlands" },
                new Language{ ID = 7, Name = "Português" },
                new Language{ ID = 8, Name = "Rumantsch" },
                new Language{ ID = 9, Name = "Türkçe" },
                new Language{ ID = 10, Name = "dansk" },
                new Language{ ID = 11, Name = "eesti" },
                new Language{ ID = 12, Name = "hrvatski" },
                new Language{ ID = 13, Name = "italiano" },
                new Language{ ID = 14, Name = "latviešu" },
                new Language{ ID = 15, Name = "Afrikaans" },
                new Language{ ID = 16, Name = "lietuvių" },
                new Language{ ID = 17, Name = "magyar" },
                new Language{ ID = 18, Name = "norsk" },
                new Language{ ID = 19, Name = "polski" },
                new Language{ ID = 20, Name = "română" },
                new Language{ ID = 21, Name = "shqipe" },
                new Language{ ID = 22, Name = "slovenski" },
                new Language{ ID = 23, Name = "slovenčina" },
                new Language{ ID = 24, Name = "suomi" },
                new Language{ ID = 25, Name = "svenska" },
                new Language{ ID = 26, Name = "Ελληνικά" },
                new Language{ ID = 27, Name = "Беларускі" },
                new Language{ ID = 28, Name = "русский" },
                new Language{ ID = 29, Name = "українська" },
                new Language{ ID = 30, Name = "日本語" },
                new Language{ ID = 31, Name = "한국어" },
                new Language{ ID = 32, Name = "íslenska (heyrnarskertir)"}
            };
            languages.ForEach(l => context.Languages.Add(l));
            context.SaveChanges();

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
                new Movie{ ID = 1, CategoryID = 2, Title = "Die Hard", ReleaseYear = 1988, Link = "http://www.imdb.com/title/tt0095016/?ref_=fn_al_tt_1", Active = true },
                new Movie{ ID = 2, CategoryID = 4, Title = "Lion King", ReleaseYear = 1995, Active = false },
                new Movie{ ID = 3, CategoryID = 3, Title = "Brokeback mountain", ReleaseYear = 2005, Active = false },
            };
            movies.ForEach(m => context.Movies.Add(m));
            context.SaveChanges();

            var shows = new List<Show>
            {
                new Show{ ID = 4, CategoryID = 4, Title = "Spongebob Squarepants", Series = 2, Episode = 13, ReleaseYear = 1999, Active = false },
                new Show{ ID = 5, CategoryID = 4, Title = "Spongebob Squarepants", Series = 2, Episode = 14, ReleaseYear = 2009, Active = false },
                new Show{ ID = 6, CategoryID = 3, Title = "House of cards", Series = 1, Episode = 3, ReleaseYear = 2013, Active = false },
                new Show{ ID = 7, CategoryID = 1, Title = "How i met your mother", Series = 7, Episode = 13, ReleaseYear = 2005, Active = false },
            };
            shows.ForEach(s => context.Shows.Add(s));
            context.SaveChanges();

            var clips = new List<Clip>
            {
                new Clip{ ID = 8, CategoryID = 1, Title = "Nyan cat", ReleaseYear = 2009, Link = "https://www.youtube.com/embed/wZZ7oFKsKzY", Active = false },
                new Clip{ ID = 9, CategoryID = 1, Title = "Charlie bit my finger", ReleaseYear = 2007, Link = "https://www.youtube.com/embed/_OBlgSz8sSM", Active = true }
            };
            clips.ForEach(c => context.Clips.Add(c));
            context.SaveChanges();

            var translations = new List<Translation>
            {
                new Translation{ ID = 1, LanguageID = 1, MediaID = 9, NumberOfDownloads = 42, Score = 0, Locked = false },
                new Translation{ ID = 2, LanguageID = 2, MediaID = 1, NumberOfDownloads = 3, Score = 1337, Locked = true },
                new Translation{ ID = 3, LanguageID = 15, MediaID = 1, NumberOfDownloads = 17, Score = 0, Locked = false },
            };
            translations.ForEach(t => context.Translations.Add(t));
            context.SaveChanges();

            var translationSegments = new List<TranslationSegment>
            {
                new TranslationSegment{ ID = 1, Timestamp = "00:01:32,777 --> 00:01:35,878", SegmentID = 1, Line1 = "Skrifaðu yfir druslan þín", Line2 = "Virkar þetta?", Original1 = "Blalblaasdf", Original2 = "asdfsad", TranslationID = 2 },
                new TranslationSegment{ ID = 2, Timestamp = "00:03:32,777 --> 00:03:35,878", SegmentID = 2, Line1 = "Blabla", Line2 = "FADklsdajf", Original1 = "Blalblaasdf", Original2 = "asdfsad", TranslationID = 2 },
                new TranslationSegment{ ID = 3, Timestamp = "00:22:32,777 --> 00:22:35,878", SegmentID = 3, Line1 = "Blabla", Line2 = "FADklsdajf", Original1 = "Blalblaasdf", Original2 = "asdfsad", TranslationID = 2 },
                new TranslationSegment{ ID = 4, Timestamp = "00:33:32,777 --> 00:33:35,878", SegmentID = 4, Line1 = "Blabla", Line2 = "FADklsdajf", Original1 = "Blalblaasdf", Original2 = "asdfsad", TranslationID = 2 },
                new TranslationSegment{ ID = 5, Timestamp = "22:56:32,777 --> 22:56:35,878", SegmentID = 5, Line1 = "Blabla", Line2 = "FADklsdajf", Original1 = "Blalblaasdf", Original2 = "asdfsad", TranslationID = 2 }
            };
            translationSegments.ForEach(ts => context.TranslationSegments.Add(ts));
            context.SaveChanges();

            var requests = new List<Request>
            {
                new Request{ ID = 1, LanguageID = 1, Score = 32, MediaID = 2 },
                new Request{ ID = 2, LanguageID = 2, Score = 18, MediaID = 8 },
                new Request{ ID = 3, LanguageID = 3, Score = 14, MediaID = 7 },
                new Request{ ID = 4, LanguageID = 4, Score = 7, MediaID = 7 },
                new Request{ ID = 5, LanguageID = 5, Score = 2, MediaID = 5 }
            };
            requests.ForEach(r => context.Requests.Add(r));
            context.SaveChanges();

			var reports = new List<Report>
			{
				new Report{ ID = 1, ReportDate = DateTime.Parse("2014-01-10 09:15:00"), ReportText = "Gaur, hvadda paela!", TranslationID = 1},
				new Report{ ID = 2, ReportDate = DateTime.Parse("2014-01-10 09:15:00"), ReportText = "Gaur, hvadda paela!", TranslationID = 2},
				new Report{ ID = 3, ReportDate = DateTime.Parse("2014-01-10 09:15:00"), ReportText = "Gaur, hvadda paela!", TranslationID = 3},
				new Report{ ID = 4, ReportDate = DateTime.Parse("2014-01-10 09:15:00"), ReportText = "Gaur, hvadda paela!", TranslationID = 3}
			};
			reports.ForEach(r => context.Reports.Add(r));
			context.SaveChanges();
        }
    }*/
}