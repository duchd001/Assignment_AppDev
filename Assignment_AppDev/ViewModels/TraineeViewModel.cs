using Assignment_AppDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment_AppDev.ViewModels
{
    public class TraineeViewModel
    {
        public Trainee Trainee { get; set; }
        public IEnumerable<ApplicationUser> Trainees { get; set; }
    }
}