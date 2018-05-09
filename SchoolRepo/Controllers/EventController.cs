using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        public IActionResult Index()
        {
            

            string name = HttpContext.Session.GetString("UserName");

            if(name != null)
            {
                int id = (int)HttpContext.Session.GetInt32("UserID");
                string grade = HttpContext.Session.GetString("UserGrade");
                Grade grades = context.Grades.First(g => g.Level == grade);
                ViewBag.gradeID = grades.ID;
                ViewBag.ID = id;
            }
            

            ViewBag.Name = name;

            return View();
        }

        public IActionResult Add()
        {
            ViewBag.Name = HttpContext.Session.GetString("UserName");
            ViewBag.Grade = HttpContext.Session.GetString("UserGrade");

            List<Grade> grades = context.Grades.ToList();
            return View(new EventViewModels(grades));
        }

        [HttpPost]
        public IActionResult Add(EventViewModels eventViewModels)
        {
            if (ModelState.IsValid) //if validation is OK
            {
                //Fetch a single grade object where ID == select ID
                Grade grades = context.Grades.Single(g=>g.ID == eventViewModels.EventID);

                Event events = new Event  //Create an event instance
                {
                    Title = eventViewModels.Title,
                    Start = eventViewModels.Start,
                    End = eventViewModels.End,
                    Grade = grades,
               
                };

                context.Events.Add(events); //add context  and save to the database
                context.SaveChanges();

                //string name = HttpContext.Request.Query["name"].ToString();
                //string grade = HttpContext.Request.Query["grade"].ToString();

                return Redirect("Index");

              

            }
            return View(eventViewModels); // if failed validation, reprompt the Add event form
        }

        public IActionResult PostEvent(int grade)
        {
            string userGrade = HttpContext.Session.GetString("UserGrade");
            
            var events = context.Events.ToList();


            if (grade != 0 && userGrade != "Teacher")
            {
                events = context.Events.Where(s => s.GradeID == grade).ToList();
            }
           
            //var events = new List<Dictionary<string, string>>();
            //events.Add(new Dictionary<string, string>() { { "Title", "Epic story" }, {"Start","2018-04-18" } });
           

            return new JsonResult(events);
            
        }

    }
}