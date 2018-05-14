using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRepo.ViewModels
{
    public class GradeViewModels
    {

        public int ID { get; set; }
        [Required]
        public string Level { get; set; }
    }
}
