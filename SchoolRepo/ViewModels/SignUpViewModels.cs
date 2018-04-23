using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolRepo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRepo.ViewModels
{
    public class SignUpViewModels
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string VerifyPassword { get; set; }
        public int StudentID { get; set; }
        public List<SelectListItem> Grade { get; set; }

        public SignUpViewModels() { } //required for model binding
        public SignUpViewModels(IEnumerable<Grade> grades)
        {
            Grade = new List<SelectListItem>();

            //<option value=1>Kindergarten</opton>
            foreach(var type in grades)
            {
                Grade.Add(new SelectListItem
                {
                    Value = type.ID.ToString(),
                    Text = type.Level.ToString(),
                });
            }
           
          
        }

    }

    
}
