using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SchoolRepo.Controllers
{
    public class TeacherController : Controller, ISession
    {
        public IActionResult Index()
        {
            ViewBag.Name = GetName();//user name
            ViewBag.Grade = GetGrade();//user grade

            return View() ;
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