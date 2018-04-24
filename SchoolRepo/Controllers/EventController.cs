using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolRepo.Data;
using SchoolRepo.Models;
using SchoolRepo.ViewModels;

namespace SchoolRepo.Controllers
{
    public class EventController : Controller
    {
        private readonly RepoDBContext context;

        public EventController(RepoDBContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index(int studentID)
        {
            if(studentID != 0)
            {
                User user = context.Users.Single(s => s.ID == studentID);
                ViewBag.Name = user.Name;
                Grade grade = context.Grades.First(g => g.Level == user.Grade);
                ViewBag.gradeID = grade.ID;
                ViewBag.ID = studentID;
            }
            

            return View();
        }

        public IActionResult Add()
        {
            List<Grade> grade = context.Grades.ToList();
            return View(new EventViewModels(grade));
        }

        [HttpPost]
        public IActionResult Add(EventViewModels eventViewModels)
        {
            if (ModelState.IsValid) //if validation is OK
            {
                //Fetch a single grade object where ID == select ID
                Grade grade = context.Grades.Single(g=>g.ID == eventViewModels.EventID);

                Event events = new Event  //Create an event instance
                {
                    Title = eventViewModels.Title,
                    Start = eventViewModels.Start,
                    End = eventViewModels.End,
                    Grade = grade,
               
                };

                context.Events.Add(events); //add context  and save to the database
                context.SaveChanges();

                return Redirect("Index");

            }
            return View(eventViewModels); // if failed validation, reprompt the Add event form
        }

        public IActionResult PostEvent(int grade)
        {
            var events = context.Events.ToList();


            if (grade != 0)
            {
                events = context.Events.Where(s => s.GradeID == grade).ToList();
            }
           
            //var events = new List<Dictionary<string, string>>();
            //events.Add(new Dictionary<string, string>() { { "Title", "Epic story" }, {"Start","2018-04-18" } });
           

            return new JsonResult(events);
            
        }

    }
}