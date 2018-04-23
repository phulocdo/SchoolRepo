using Microsoft.EntityFrameworkCore;
using SchoolRepo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRepo.Data
{
    public class RepoDBContext : DbContext
    {
        public DbSet<File> Files { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Grade> Grades { get; set; }

        public RepoDBContext(DbContextOptions<RepoDBContext> dbContextOptions)
            : base(dbContextOptions)
        { }


        
    }
}
