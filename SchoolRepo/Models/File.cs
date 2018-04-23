using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRepo.Models
{
    public class File
    {
        public int ID { get; set; }
        public string FileName { set; get; }
        public string Description { set; get; }
        public string FilePath { get; set; }
        //public byte[] Content { get; set; }
        public int StudentID { get; set; }
        public Student Students {get;set;} //navigation property

    }
}
