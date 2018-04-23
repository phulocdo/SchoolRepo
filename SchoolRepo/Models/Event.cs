using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRepo.Models
{
    public class Event
    {
        public int ID {get;set;}
        public string Title { set; get; }
        public DateTime Start { set; get; }
        public DateTime End { set; get; }
        public int GradeID { set; get; }
        public Grade Grade { set; get; } //navigation property
        
    }
}
