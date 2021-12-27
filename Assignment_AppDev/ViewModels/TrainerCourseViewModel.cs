using Assignment_AppDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment_AppDev.ViewModels
{
    public class TrainerCourseViewModel
    {
        public string TrainerId { get; set; }
        public List<Trainer> Trainers { get; set; }
        public Trainer Trainer { get; set; }
        public Course Course { get; set; }
        public int CourseId { get; set; }
        public List<Course> Courses { get; set; }
    }
}