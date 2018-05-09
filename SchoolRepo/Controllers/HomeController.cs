using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolRepo.Data;
using SchoolRepo.Models;
using SchoolRepo.ViewModels;

namespace SchoolRepo.Controllers
{
    public class HomeController : Controller
    {
        private RepoDBContext contex;

        public HomeController(RepoDBContext dBContext)
        {
            contex = dBContext;
        }

        public IActionResult Index()
        {
            List<Access> access = contex.Accesses.ToList();
            return View(access);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Add()
        {
            AccessViewModels accessViewModels = new AccessViewModels();
            return View(accessViewModels);
        }

        [HttpPost]
        public IActionResult Add(AccessViewModels accessViewModels)
        {
            if (ModelState.IsValid)
            {
                Access access = new Access
                {
                    ID=accessViewModels.ID,
                    Name=accessViewModels.Name,
                    Code=accessViewModels.Code
                };

                contex.Accesses.Add(access);
                contex.SaveChanges();

                return Redirect("index");
            }

            return View(accessViewModels);
        }


    }
}
