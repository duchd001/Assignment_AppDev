using Assignment_AppDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment_AppDev.ViewModels
{
    public class TraineeCourseViewModel
    {
        public string TraineeId { get; set; }
        public List<Trainee> Trainees { get; set; }
        public Trainee Trainee { get; set; }
        public Course Course { get; set; }
        public int CourseId { get; set; }
        public List<Course> Courses { get; set; }
        
    }
}