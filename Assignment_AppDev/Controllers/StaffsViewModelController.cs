using Assignment_AppDev.Models;
using Assignment_AppDev.Utils;
using Assignment_AppDev.ViewModels;
using System.Data.Entity.Migrations;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace Assignment_AppDev.Controllers
{
	
	public class StaffsViewModelsController : Controller
	{
		private ApplicationDbContext _context;
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;
		public StaffsViewModelsController()
		{
			_context = new ApplicationDbContext();
		}
		public StaffsViewModelsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
		{
			UserManager = userManager;
			SignInManager = signInManager;
		}
		public ApplicationSignInManager SignInManager
		{
			get
			{
				return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
			}
			private set
			{
				_signInManager = value;
			}
		}

		public ApplicationUserManager UserManager
		{
			get
			{
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				_userManager = value;
			}
		} //Get from AccountController
		  // GET: StaffViewModels
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Index()
		{
			var traineeRole = (from te in _context.Roles where te.Name.Contains("Trainee") select te).FirstOrDefault();
			// Get User role name Trainee and return
			var traineeUser = _context.Users.Where(u => u.Roles.Select(teus => teus.RoleId).Contains(traineeRole.Id)).ToList();
			// Get the user in the User table roles as Trainee and return list
			var traineeUserVM = traineeUser.Select(user => new StaffViewModel
			// return list out to VM
			{
				UserName = user.UserName,
				Email = user.Email,
				RoleName = "Trainee",
				UserID = user.Id
			}).ToList();
			var trainerRole = (from t in _context.Roles where t.Name.Contains("Trainer") select t).FirstOrDefault();
			var trainerUser = _context.Users.Where(u => u.Roles.Select(tnus => tnus.RoleId).Contains(trainerRole.Id)).ToList();
			var trainerUserVM = trainerUser.Select(user => new StaffViewModel
			{
				UserName = user.UserName,
				Email = user.Email,
				RoleName = "Trainer",
				UserID = user.Id
			}).ToList();
			var staff = new StaffViewModel { Trainee = traineeUserVM, Trainer = trainerUserVM };
			return View(staff);

		}

		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Edit(string id)
		{

			var editUser = _context.Users.Find(id);
			if (editUser == null)
			{
				return HttpNotFound();
			}
			return View(editUser);
		}

		[HttpPost]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Edit(ApplicationUser user)
		{
			var userInDb = _context.Users.Find(user.Id);

			if (userInDb == null)
			{
				return View(user);
			}

			if (ModelState.IsValid)
			{

				//userInDb.PhoneNumber = user.PhoneNumber;
				userInDb.Email = user.Email;

				_context.Users.AddOrUpdate(userInDb);
				_context.SaveChanges();

				return RedirectToAction("Index");
			}
			return View(user);

		}
		[Authorize(Roles = "TrainingStaff")]
        public ActionResult Delete(string id)

        {
            var userInDb = _context.Users.SingleOrDefault(p => p.Id == id);

            if (userInDb == null)
            {
                return HttpNotFound();
            }
            _context.Users.Remove(userInDb);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult CreateTraineeAccount()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "TrainingStaff")]
		public async Task<ActionResult> CreateTraineeAccount(RegisterViewModel model)
		{
			//Borrow from AccountController
			if (ModelState.IsValid)
			{
				var traineeInf = new ApplicationUser
				{
					UserName = model.UserName,
					Email = model.Email,
					PhoneNumber = model.PhoneNumber
				};
				var result = await UserManager.CreateAsync(traineeInf, model.Password);
				if (result.Succeeded)
				{
					//Add to trainee role
					await UserManager.AddToRoleAsync(traineeInf.Id, Role.Trainee);
					return RedirectToAction("Index", "StaffsViewModels");
				}
				AddErrors(result);
			}
			return View(model);

		}
		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}
	}
}