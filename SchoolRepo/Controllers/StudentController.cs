using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolRepo.Data;
using SchoolRepo.Models;
using SchoolRepo.ViewModels;

namespace SchoolRepo.Controllers
{
    public class StudentController : Controller, ISession
    {
        //mechanism with which we interact with object store in database
        private static RepoDBContext context;

        //MVC will create in instance of RepoDbContext and passing it to our controller's contructor
        public StudentController(RepoDBContext dbContext)
        {
            context = dbContext;
            
        }
        public IActionResult Index(string name, string grade)
        {

            ViewBag.Name = GetName();//user name
            ViewBag.Grade = GetGrade();//user grade
            List<Student> student = context.Students.ToList();

            return View(student);
        }

        public IActionResult Add()
        {
           
            ViewBag.Name = GetName();//user name
            ViewBag.Grade = GetGrade();//user grade
            AddStudentViewModels addStudentViewModels = new AddStudentViewModels();

            return View(addStudentViewModels);
        }

        [HttpPost]
        public IActionResult Add(AddStudentViewModels addStudentViewModels)
        {
            if (ModelState.IsValid) //if model validation if valid, add object to the database
            {
                Student student = new Student
                {
                    ID = addStudentViewModels.ID,
                    Name = addStudentViewModels.Name,
                };
                context.Students.Add(student);
                context.SaveChanges();

                
                return Redirect("Index");
            }

           ViewBag.Name = GetName(); //user name
           ViewBag.Grade = GetGrade(); //user grade

            return View(addStudentViewModels);
        }

        public IActionResult Remove(string name, string grade)
        {
            ViewBag.Name = GetName();//user name
            ViewBag.Grade = GetGrade();//user grade
            List<Student> student = context.Students.ToList();

            return View(student);
        }

        //delete student(s) from the database
        [HttpPost]
        public IActionResult Remove(int[] studentIDs)
        {
            foreach(int id in studentIDs)
            {
                Student student = context.Students.Single(s=>s.ID == id);
                context.Students.Remove(student);
            }
            context.SaveChanges();

           
            return Redirect("Index");
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