using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment_AppDev.Models
{
    public class AssignTraineeToCourse
    {
        public int ID { get; set; }
        public string TraineeID { get; set; }
        public ApplicationUser Trainee { get; set; }
        public int CourseID { get; set; }
        public Course Course { get; set; }
    }
}