using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assignment_AppDev.Models
{
    public class Trainee
    {
        public int Id { get; set; }
        [DisplayName("Trainee")]
        [Required]
        public string TraineeID { get; set; }

        [DisplayName("Trainee Name")]
        public string Name { get; set; }
        public string Email { get; set; }
        public ApplicationUser Trainees { get;set; }

    }
}