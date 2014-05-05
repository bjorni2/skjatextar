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
    }

    public class ApplicationInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            // Example of test data

            //var students = new List<Student>
            //{
            //new Student{FirstMidName="Carson",LastName="Alexander",EnrollmentDate=DateTime.Parse("2005-09-01")},
            //new Student{FirstMidName="Meredith",LastName="Alonso",EnrollmentDate=DateTime.Parse("2002-09-01")},
            //};

            //students.ForEach(s => context.Students.Add(s));
            //context.SaveChanges();
        }
    }
}