﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assignment_AppDev.Models
{
    public class Trainer
    {
		public int ID { get; set; }		

		[DisplayName("Trainer ID")]
		[Required]
		public int TrainerID { get; set; }

		[DisplayName("Trainer Name")]
		public int Age { get; set; }	
		public string Name { get; set; }
		public int Phone { get; set; }
		public string Address { get; set; }
        public string Email { get; set; }
		public ApplicationUser Trainers { get; set; }
	}
}