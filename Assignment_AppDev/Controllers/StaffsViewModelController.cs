using Assignment_AppDev.Models;
using Assignment_AppDev.Utils;
using Assignment_AppDev.ViewModels;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;

namespace Assignment_AppDev.Controllers
{
	public class StaffsViewModelsController : Controller
	{
		ApplicationDbContext _context;
		public StaffsViewModelsController()
		{
			_context = new ApplicationDbContext();
		}
        // GET: StaffViewModels
        [Authorize(Roles = Role.TrainingStaff)]
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
            var staff = new StaffViewModel { Trainee = traineeUserVM };
			return View(staff);
		}

		[HttpGet]
		[Authorize(Roles = Role.TrainingStaff)]
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
		[Authorize(Roles = Role.TrainingStaff)]
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

		[Authorize(Roles = Role.TrainingStaff)]
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
	}
}