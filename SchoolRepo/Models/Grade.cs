using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRepo.Models
{
    public class Grade
    {
        public int ID { get; set; }
        public string Level { get; set; }
        public List<Event> Events { get; set; }
    }
}
