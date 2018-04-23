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
        private RepoDBContext context;

        public GradeController(RepoDBContext dBContext)
        {
            context = dBContext;
        }

        public IActionResult Index()
        {

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
            if (ModelState.IsValid)
            {

                Grade grade = new Grade
                {
                    ID = gradeViewModel.ID,
                    Level = gradeViewModel.Level,
                    
                };
                context.Grades.Add(grade);
                context.SaveChanges();

                return Redirect("Index");
            }
            return View(gradeViewModel);
        }
    }
}