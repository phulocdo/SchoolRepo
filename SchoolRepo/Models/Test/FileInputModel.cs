﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SchoolRepo.Models.Test
{
    public class FileInputModel
    {
        public IFormFile FileToUpload { get; set; }
    }
}
