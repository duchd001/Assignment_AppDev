using Assignment_AppDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment_AppDev.ViewModels
{
    public class AssignTrainerToCourseViewModel
    {
        public AssignTrainerToCourse AssignTrainerToCourse { get; set; }
        public IEnumerable<ApplicationUser> Trainers { get; set; }
        public IEnumerable<Course> Courses { get; set; }
    }
}