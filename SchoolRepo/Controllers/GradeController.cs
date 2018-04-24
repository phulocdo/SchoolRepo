using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolRepo.Data;
using SchoolRepo.Models;
using SchoolRepo.ViewModels;

namespace SchoolRepo.Controllers
{
    public class GradeController : Controller
    {
        //context to access object stored in the database
        private RepoDBContext context;

        public GradeController(RepoDBContext dBContext)
        {
            context = dBContext;
        }

        public IActionResult Index()
        {
            //list of grade
            List<Grade> grade = context.Grades.ToList();

            return View(grade);
        }

        public IActionResult Add()
        {
            GradeViewModels gradeViewModels = new GradeViewModels();
            return View(gradeViewModels);
        }

        [HttpPost]
        public IActionResult Add(GradeViewModels gradeViewModel)
        {
            if (ModelState.IsValid) //if form validation is valid,then
            {
                //instantiate a grade object
                Grade grade = new Grade
                {
                    ID = gradeViewModel.ID,
                    Level = gradeViewModel.Level,
                    
                };
                context.Grades.Add(grade); //save context to the database
                context.SaveChanges();

                return Redirect("Index");
            }
            return View(gradeViewModel); //if model state failed, prompt the add grade form
        }
    }
}