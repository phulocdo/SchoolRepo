using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SchoolRepo.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Name = HttpContext.Session.GetString("UserName");
            ViewBag.Grade = HttpContext.Session.GetString("UserGrade");

            return View() ;
        }

        
    }
}