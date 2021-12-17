using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment_AppDev.ViewModels
{
    public class StaffViewModel
    {
		public string UserID { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string RoleName { get; set; }
		public List<StaffViewModel> Trainee { get; set; }
		public List<StaffViewModel> Trainer { get; set; }
	}
}