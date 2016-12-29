using System;
using System.Collections.Generic;
using AMPSystem.Classes;

namespace AMPSystem.DAL
{
    public class AmpInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<AmpContext>
    {
        protected override void Seed(AmpContext context)
        {
            /*
            var users = new List<User>
            {
            new User{Name = "Ana", Email = "ana@hotmail.com"},
            new User{Name = "Filipa",Email ="filipa.kk@hotmail.com"}
            };
            users.ForEach(s => context.Users.Add(s));
            context.SaveChanges();

            var alerts = new List<Alert>
            {
            new Alert {Time = TimeSpan.Zero, Item = null}
            };
            alerts.ForEach(s => context.Alerts.Add(s));
            context.SaveChanges();

            var lessons = new List<Lesson>
            {
            new Lesson {Name="DIS", Color = "Azul", Type = "TP", StartTime = DateTime.Now, EndTime = DateTime.MaxValue},
            new Lesson {Name="EI", Color = "Azul", Type = "T", StartTime = DateTime.MaxValue, EndTime = DateTime.MaxValue}
            };
            lessons.ForEach(s => context.Lessons.Add(s));
            context.SaveChanges();

            var evaluationMoment = new List<EvaluationMoment>
            {
            new EvaluationMoment {Name="DIS", Color = "Azul", StartTime = DateTime.Now, EndTime = DateTime.MaxValue},
            new EvaluationMoment {Name="EI", Color = "Azul", StartTime = DateTime.MaxValue, EndTime = DateTime.MaxValue}
            };
            evaluationMoment.ForEach(s => context.EvalMoments.Add(s));
            context.SaveChanges();

            var officeH = new List<OfficeHours>
            {
            new OfficeHours {Name="DIS", Color = "Azul", StartTime = new DateTime(2016, 3, 1), EndTime = new DateTime(2016, 3, 2)},
            new OfficeHours {Name="EI", Color = "Azul", StartTime = new DateTime(2016, 3, 2), EndTime = new DateTime(2016, 3, 2)}
            };
            officeH.ForEach(s => context.OfficeHours.Add(s));
            context.SaveChanges();
            */
        }

    }
}