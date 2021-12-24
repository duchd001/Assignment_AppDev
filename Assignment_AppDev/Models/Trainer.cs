using System;
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
		public string TrainerID { get; set; }
		
		[DisplayName(" Name")]
		public string Name { get; set; }
		
		[DisplayName("Address")]
		public string Address { get; set; }
		public int Age { get; set; }
		public int Phone { get; set; }
		
		public ApplicationUser Trainers { get; set; }
	}
}