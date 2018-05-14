using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolRepo.Data;
using SchoolRepo.Models;
using SchoolRepo.ViewModels;

namespace SchoolRepo.Controllers
{
    public class UserController : Controller, ISession
    {
        //context to access object stored in the database
        private RepoDBContext context;

       
        //MVC will create dbcontext and hand it to the controller's constructor
        public UserController(RepoDBContext dbContext)
        {
            context = dbContext;
        }
        
        public IActionResult Index()
        {
            string name = GetName();//user name

            if(name != null)
            {
                ViewBag.ID = GetID() ;
                ViewBag.Name = name;
                
            }

            return View();
        }

        public IActionResult SignUp()
        {
            List<Grade> grade = context.Grades.ToList();
            return View(new SignUpViewModels(grade));
        }

        [HttpPost]
        public IActionResult SignUp(SignUpViewModels signUpViewModels)
        {
            List<Grade> grades = context.Grades.ToList();

            if (ModelState.IsValid)
            {
                //make sure Password and verifyPassword are the same
                if (signUpViewModels.Password != signUpViewModels.VerifyPassword)
                {
                    ModelState.AddModelError("VerifyPassword", "Password Mismatched!");
                    return View(new SignUpViewModels(grades));
                }
                else
                {
                    User id = context.Users.Find(signUpViewModels.ID);

                    //veriry if user id is already in the DB
                    if(id != null)
                    {
                        ModelState.AddModelError("ID", "User ID already exist!");
                        return View(new SignUpViewModels(grades));
                    }



                    Grade grade = context.Grades.Single(s => s.ID == signUpViewModels.StudentID);
                    User user = new User
                    {
                        //add user to the database
                        ID = signUpViewModels.ID,
                        Name = signUpViewModels.Name,
                        Password = DeriveSaltHashKey(signUpViewModels.Password),
                        Grade = grade.Level
                        
                    };

                    //save to the database
                    context.Users.Add(user);
                    context.SaveChanges();


                    return Redirect("Index");
                }

            }

            

            return View(new SignUpViewModels(grades));
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModels loginViewModels)
        {
            if (ModelState.IsValid)
            {
                User user = context.Users.Find(loginViewModels.ID);
                //account does not exist, redirect to login page
                if (user == null)
                {
                    ModelState.AddModelError("ID", "Student ID does not exist");
                    return View(loginViewModels);
                }

                //split salt and hash key
                string[] saltHash = user.Password.Split(";");

                //incorrect password
                if ( !IsPasswordValid(loginViewModels.Password,saltHash[0],saltHash[1]))
                {
                    ModelState.AddModelError("Password", "Incorrect Password");
                    return View(loginViewModels);
                }

                if (user.Grade.Equals("Teacher"))
                {
                    Access access = context.Accesses.Find(loginViewModels.ID);
                    //account does not exist, redirect to login page
                    if (user == null)
                    {
                        ModelState.AddModelError("ID", "Invalid access code");
                        return View(loginViewModels);
                    }

                    //manage user session data
                    HttpContext.Session.SetString("UserName", user.Name);
                    HttpContext.Session.SetInt32("UserID", loginViewModels.ID);
                    HttpContext.Session.SetString("UserGrade", user.Grade);

                    return Redirect("/Teacher/Index");

                   // return Redirect("/Teacher/Index?name=" + user.Name + "&grade=" + user.Grade);

                }

                //user session
                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetInt32("UserID", loginViewModels.ID);
                HttpContext.Session.SetString("UserGrade", user.Grade);

                return Redirect("Index");
            }

            return View(loginViewModels);
        }

        public IActionResult Logout()
        {
            //clear all entries from the current session
            HttpContext.Session.Clear();

            return View("Login");
        }


        //derive salt + hash key
        private string DeriveSaltHashKey(string password)
        {
            int saltSize = 256;  //number of bytes of salt
            int iteration = 1000; //number of iteration, the advantage of making bruteforce more painfull
            int hashSize = 20; //number of byte of hash
            

            //derive the key
            var deriveBytes = new Rfc2898DeriveBytes(password, saltSize, iteration);
            byte[] salt = deriveBytes.Salt;
            byte[] hash = deriveBytes.GetBytes(hashSize);

            
            string saltHash = Convert.ToBase64String(salt) + ";" + Convert.ToBase64String(hash) ;

            return saltHash;
        }


        //Password validation
        private bool IsPasswordValid(string password, string salt,string hash_orig)
        {
            int hashSize = 20; //number of byte of hash
            int iteration = 1000; //number of iteration, the advantage of making bruteforce more painfull

            byte[] saltBytes = Convert.FromBase64String(salt);

            //drive new hash with password and salt 
            var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, iteration);
            byte[] hashBytes = deriveBytes.GetBytes(hashSize);
            string hash_new = Convert.ToBase64String(hashBytes);

            //compare hash keys
            if (hash_new.Equals(hash_orig))
            {
                return true;
            }
            else
            {
                return false;
            }
            
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