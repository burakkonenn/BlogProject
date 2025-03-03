﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Context
{
    public class Category
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Yayınınız için lütfen etiket belirleyin")]
        public string Name { get; set; }

        public List<Blog> Blogs { get; set; }
    }
}
