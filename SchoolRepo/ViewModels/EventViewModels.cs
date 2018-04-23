using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolRepo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRepo.ViewModels
{
    public class EventViewModels
    {

        public int EventID { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<SelectListItem> Grade { get; set; }

        //defult constructor is required to model binding
        public EventViewModels() { }

        public EventViewModels(IEnumerable<Grade> gradeCategories)
        {
            Grade = new List<SelectListItem>();
            
            foreach(var type in gradeCategories)
            {

               // <option value="0">Hard</option>
                Grade.Add(new SelectListItem
                {
                    Value = type.ID.ToString(),
                    Text = type.Level.ToString()
                });
             }
        }
    }

    
}



