using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SkjaTextar.Models;

namespace SkjaTextar.DAL
{
    public class SubTitleInitializer : DropCreateDatabaseIfModelChanges<SubTitleContext>
    {
        protected override void Seed(SubTitleContext context)
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