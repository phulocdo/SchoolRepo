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
    public class EventController : Controller, ISession
    {

        //context to access object stored in the data base
        private readonly RepoDBContext context;

        //MVC will initiate dbContext and  hand control to the constructor
        public EventController(RepoDBContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            

            string name = GetName(); //user name
            string grade = GetGrade();

            if (name != null)
            {
                //int id = GetID();
                Grade level = context.Grades.First(g => g.Level == grade);
                ViewBag.gradeID = level.ID;
                ViewBag.ID = GetID();
                ViewBag.Grade = GetGrade();
            }
            

            ViewBag.Name = name;

            return View();
        }

        public IActionResult Add()
        {
            ViewBag.Name = GetName() ;//user name   
            ViewBag.Grade = GetGrade(); //user grade
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

                return Redirect("Index");

            }
            List<Grade> grad = context.Grades.ToList();

            return View(new EventViewModels(grad)); // if failed validation, reprompt the Add event form
        }

        public IActionResult PostEvent(int grade)
        {
            string userGrade = GetGrade(); //grade level
            
            var events = context.Events.ToList();


            if (grade != 0 && userGrade != "Teacher")
            {
                events = context.Events.Where(s => s.GradeID == grade).ToList();
            }
           
            //var events = new List<Dictionary<string, string>>();
            //events.Add(new Dictionary<string, string>() { { "Title", "Epic story" }, {"Start","2018-04-18" } });
           

            return new JsonResult(events);
            
        }

        //Return user name
        public string GetName()
        {
            return HttpContext.Session.GetString("UserName");
        }

        //return user grade level
        public string GetGrade()
        {
            return HttpContext.Session.GetString("UserGrade");
        }

        //return user ID
        public int GetID()
        {
            return (int)HttpContext.Session.GetInt32("UserID");
        }

    }
}