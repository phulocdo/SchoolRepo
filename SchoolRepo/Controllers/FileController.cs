using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolRepo.Data;
using SchoolRepo.Models;
using SchoolRepo.ViewModels;

namespace SchoolRepo.Controllers
{
    public class FileController : Controller,ISession
    {
        //environment where files will be upload
        private IHostingEnvironment environment;

        //context to access object stored in the database
        private RepoDBContext context;

        //MVC will create dbcontext and hand it to the controller's constructor
        public FileController(RepoDBContext dbContext, IHostingEnvironment hostingEnvironment)
        {
            context = dbContext;
            environment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            
            string name = GetName(); //user Name
            string grade = GetGrade(); //user Grade level
            var files = context.Files.ToList();

            if (name != null)
            {
                int id = (int)HttpContext.Session.GetInt32("UserID");
                if (grade != "Teacher")
                {
                    
                    files = context.Files.Where(f => f.StudentID == id).Include(s => s.Students).ToList();
                }
                
                ViewBag.ID = id; //user ID
                ViewBag.Name = name;
                ViewBag.Grade = grade;
            }
            

            return View(files);
        }

        public IActionResult Upload()
        {
            ViewBag.Name = GetName(); //user name
            ViewBag.Grade = GetGrade();//user grade level
            
            //drop down list of students
            List<Student> studentList = context.Students.ToList();

            return View(new AddFileViewModels(studentList));
        }

        [HttpPost]
        public async Task<IActionResult> Upload(AddFileViewModels addFileViewModels)
        {
            if (ModelState.IsValid)
            {
                //path where file will be uploaded
                var filePath = Path.Combine(environment.WebRootPath, "uploads");

                //fetch a single student object where ID == select ID
                Student student = context.Students.Single(s => s.ID == addFileViewModels.StudentID);

                foreach (var file in addFileViewModels.files)
                {
                    if (file.Length > 0)
                    {
                        //Add file to existing list
                        Models.File newFile = new Models.File
                        {
                            FileName = file.FileName,
                            FilePath = filePath,
                            Description = addFileViewModels.Description,
                            Students = student,
                        };
                        //save file to filesystem
                        using (var fileStream = new FileStream(Path.Combine(filePath, file.FileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        context.Files.Add(newFile);
                    }

                }
                context.SaveChanges(); //save to the database

                return Redirect("Index");
            }

            List<Student> studentList = context.Students.ToList();
            ViewBag.Name = GetName(); //user name
            ViewBag.Grade = GetGrade();//user grade level

            return View(new AddFileViewModels(studentList));

        }

        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                          Directory.GetCurrentDirectory(),
                           "wwwroot\\uploads", filename);


            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            //return FileResult with either a stream,byte[] or virtual path of the file
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        //return content-type of the file
        private string GetContentType(string path)
        {
            var types = GetTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        //list of support files type
        private Dictionary<string, string> GetTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        public IActionResult Remove()
        {
            ViewBag.Name = GetName();//user name
            ViewBag.Grade = GetGrade();//user grade level

            List<Models.File> files = context.Files.ToList();

            return View(files);
        }

        [HttpPost]
        //Delete file(s) from the database
        public IActionResult Remove(int[] fileIDs)
        {

            foreach (int id in fileIDs)
            {
                Models.File file = context.Files.Single(f => f.ID == id);
                DeleteFile(file.FilePath + file.FileName);
                context.Files.Remove(file);
            }
            context.SaveChanges();

            
            return Redirect("Index");
        }

        //remove files from the database server
        private void DeleteFile(string path)
        {
            System.IO.File.Delete(path);
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