using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolRepo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRepo.ViewModels
{
    public class AddFileViewModels
    {
        
        //public string FileName { get; set;}
        
        //public IFormFile File { get; set; }
       // [Required(ErrorMessage ="Must select file to upload")]
        public int StudentID { get; set; }
        public ICollection<IFormFile> files { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public List<SelectListItem> Students { get; set; }

        public AddFileViewModels() { }

        public AddFileViewModels(IEnumerable<Student> students) {

            Students = new List<SelectListItem>();
            foreach(var student in students)
            {
                // <option value="55555">John</option>
                Students.Add(new SelectListItem
                {
                    Value = student.ID.ToString(),
                    Text = student.Name

                });
                    
              
            }

        }

    }
}
