using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRepo.Controllers
{
    interface ISession
    {
        string GetName(); //get user name
        string GetGrade(); //get user grade
        int GetID(); //get user ID
    }
}
