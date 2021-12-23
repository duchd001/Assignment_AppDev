using Assignment_AppDev.Models;
using Assignment_AppDev.Utils;
using Assignment_AppDev.ViewModels;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Assignment_AppDev.Controllers
{
	public class AssignTrainertoCoursesController : Controller
	{
		private ApplicationDbContext _context;
		public AssignTrainertoCoursesController()
		{
			_context = new ApplicationDbContext();
		}

		[HttpGet]
		[Authorize(Roles = "TrainingStaff, Trainee")]
		// GET: AssignTrainertoCourses
		public ActionResult Index()
		{
			if (User.IsInRole("TrainingStaff"))
			{
				var viewAssign = _context.AssignTrainerToCourses.Include(a => a.Course).Include(a => a.Trainer).ToList();
				return View(viewAssign);
			}
			if (User.IsInRole("Trainer"))
			{
				var traineeId = User.Identity.GetUserId();
				var traineeVM = _context.AssignTrainerToCourses.Where(te => te.TrainerID == traineeId).Include(te => te.Course).ToList();
				return View(traineeVM);
			}
			return View();
		}

		//GET: Trainer and Course
		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Create()
		{
			var trainerInDb = (from te in _context.Roles where te.Name.Contains("Trainer") select te).FirstOrDefault();
			// Get User role name Trainer and return
			var trainerUser = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(trainerInDb.Id)).ToList();
			// Get User in table and select the ID containing the TraineeID
			var courses = _context.Courses.ToList();

			var viewModel = new AssignTrainerToCourseViewModel
			{
				Courses = courses,
				Trainers = trainerUser,
				AssignTrainerToCourse = new AssignTrainerToCourse()
			};
			return View(viewModel);
		}


		[HttpPost]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Create(AssignTrainerToCourseViewModel assign)
		{
			var trainerInDb = (from te in _context.Roles where te.Name.Contains("Trainer") select te).FirstOrDefault();
			var trainerUser = _context.Users.Where(u => u.Roles.Select(us => us.RoleId).Contains(trainerInDb.Id)).ToList();
			var course = _context.Courses.ToList();

			if (ModelState.IsValid)
			{
				var checkTraineeAndCourseExist = _context.AssignTrainerToCourses.Include(t => t.Course).Include(t => t.Trainer)
					.Where(t => t.Course.ID == assign.AssignTrainerToCourse.CourseID && t.Trainer.Id == assign.AssignTrainerToCourse.TrainerID);
				//GET CourseID and TrainerID from the Course and Trainer tables in the ViewModel

				if (checkTraineeAndCourseExist.Count() > 0) //list ID comparison, if count == 0. jump to else
				{
					ModelState.AddModelError("", "Assign Already Exists");
				}
				else
				{
					_context.AssignTrainerToCourses.Add(assign.AssignTrainerToCourse);
					_context.SaveChanges();
					return RedirectToAction("Index");
				}
			}
			AssignTrainerToCourseViewModel traineecourseVM = new AssignTrainerToCourseViewModel()
			{
				Courses = course,
				Trainers = trainerUser,
				AssignTrainerToCourse = assign.AssignTrainerToCourse
			};
			return View(traineecourseVM);
		}


		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Delete(int id)
		{
			var assignInDb = _context.AssignTrainerToCourses.SingleOrDefault(a => a.ID == id);
			if (assignInDb == null)
			{
				return HttpNotFound();
			}

			_context.AssignTrainerToCourses.Remove(assignInDb);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}