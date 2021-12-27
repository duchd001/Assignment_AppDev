using Assignment_AppDev.Models;
using Assignment_AppDev.Utils;
using Assignment_AppDev.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment_AppDev.Controllers
{
    public class AssignTraineeToCoursesController : Controller
    {
        private ApplicationDbContext _context;
        public AssignTraineeToCoursesController()
        {
            _context = new ApplicationDbContext();
        }
        [HttpGet]
        [Authorize(Roles = "TrainingStaff, Trainee")]

        // GET: AssignTraineeToCourses
        public ActionResult Index()
        {
            if (User.IsInRole("TrainingStaff"))
            {
                var viewAssign = _context.AssignTraineeToCourses.Include(a => a.Course).Include(a => a.Trainee).ToList();
                return View(viewAssign);
            }
            if (User.IsInRole("Trainee"))
            {
                var traineeId = User.Identity.GetUserId();
                var traineeVM = _context.AssignTraineeToCourses.Where(te => te.TraineeID == traineeId).Include(te => te.Course).ToList();
                return View(traineeVM);
            }
            return View();
        }
        //GET: Trainee and Course
        [HttpGet]
        [Authorize(Roles = "TrainingStaff")]
        public ActionResult Create()
        {
            var traineeInDb = (from te in _context.Roles where te.Name.Contains("Trainee") select te).FirstOrDefault();
            // Get User role name Trainee and return
            var traineeUser = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(traineeInDb.Id)).ToList();
            // Get User in table and select the ID containing the TraineeID
            var courses = _context.Courses.ToList();
            var viewModel = new AssignTraineeToCourseViewModel
            {
                Courses = courses,
                Trainees = traineeUser,
                AssignTraineeToCourse = new AssignTraineeToCourse()
            };
            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "TrainingStaff")]
        public ActionResult Create(AssignTraineeToCourseViewModel assign)
        {
            var traineeInDb = (from te in _context.Roles where te.Name.Contains("Trainee") select te).FirstOrDefault();
            var traineeUser = _context.Users.Where(u => u.Roles.Select(us => us.RoleId).Contains(traineeInDb.Id)).ToList();
            var course = _context.Courses.ToList();

            if (ModelState.IsValid)
            {
                var checkTraineeAndCourseExist = _context.AssignTraineeToCourses.Include(t => t.Course).Include(t => t.Trainee)
                    .Where(t => t.Course.ID == assign.AssignTraineeToCourse.CourseID && t.Trainee.Id == assign.AssignTraineeToCourse.TraineeID);
                //GET CourseID and TraineeID from the Course and Trainee tables in the ViewModel

                if (checkTraineeAndCourseExist.Count() > 0) //list ID comparison, if count == 0. jump to else
                {
                    ModelState.AddModelError("", "Assign Already Exists");
                }
                else
                {
                    _context.AssignTraineeToCourses.Add(assign.AssignTraineeToCourse);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            AssignTraineeToCourseViewModel traineecourseVM = new AssignTraineeToCourseViewModel()
            {
                Courses = course,
                Trainees = traineeUser,
                AssignTraineeToCourse = assign.AssignTraineeToCourse
            };
            _context.SaveChanges();
            return View(traineecourseVM);
        }
        [HttpGet]
        [Authorize(Roles = "TrainingStaff")]
        public ActionResult Delete(int id)
        {
            var assignInDb = _context.AssignTraineeToCourses.SingleOrDefault(a => a.ID == id);
            if (assignInDb == null)
            {
                return HttpNotFound();
            }

            _context.AssignTraineeToCourses.Remove(assignInDb);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult TraineeViewTrainee(int id)
        {

            var newusers = _context.AssignTraineeToCourses.Where(u => u.Course.ID == id).Select(u => u.Trainee).ToList();
            return View(newusers);

        }

    }
}