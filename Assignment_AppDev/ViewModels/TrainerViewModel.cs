using Assignment_AppDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment_AppDev.ViewModels
{
    public class TrainerViewModel
    {
        public Trainer Trainer { get; set; }
        public IEnumerable<ApplicationUser> Trainers { get; set; }
    }
}