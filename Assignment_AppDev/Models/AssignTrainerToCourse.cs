using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment_AppDev.Models
{
    public class AssignTrainerToCourse
    {
        public int ID { get; set; }
        public string TrainerID { get; set; }
        public ApplicationUser Trainer { get; set; }
        public int CourseID { get; set; }
        public Course Course { get; set; }
    }
}