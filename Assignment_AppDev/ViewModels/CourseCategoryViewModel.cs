using Assignment_AppDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment_AppDev.ViewModels
{
    public class CourseCategoryViewModel
    {
        public Course Course { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}