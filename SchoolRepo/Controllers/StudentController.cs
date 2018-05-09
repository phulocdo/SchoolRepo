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
    public class StudentController : Controller
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

            ViewBag.Name = HttpContext.Session.GetString("UserName");
            ViewBag.Grade = HttpContext.Session.GetString("UserGrade");
            List<Student> student = context.Students.ToList();

            return View(student);
        }

        public IActionResult Add()
        {
            ViewBag.Name = HttpContext.Session.GetString("UserName");
            ViewBag.Grade = HttpContext.Session.GetString("UserGrade");
            //ViewBag.ID = HttpContext.Session.GetString("UserID");

            AddStudentViewModels addStudentViewModels = new AddStudentViewModels();

            return View(addStudentViewModels);
        }

        [HttpPost]
        public IActionResult Add(AddStudentViewModels addStudentViewModels)
        {
            if (ModelState.IsValid)
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

            return View(addStudentViewModels);
        }

        public IActionResult Remove(string name, string grade)
        {
            ViewBag.Name = HttpContext.Session.GetString("UserName");
            ViewBag.Grade = HttpContext.Session.GetString("UserGrade");
            List<Student> student = context.Students.ToList();

            return View(student);
        }

        //delete student/students from the database
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
            

    }
}