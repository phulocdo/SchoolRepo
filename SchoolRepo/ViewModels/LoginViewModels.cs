using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRepo.ViewModels
{
    public class LoginViewModels
    {
        [Required(ErrorMessage = "Student ID is required")]
        public int ID { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
