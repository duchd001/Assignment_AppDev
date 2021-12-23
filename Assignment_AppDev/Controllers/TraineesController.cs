using Assignment_AppDev.Models;
using Assignment_AppDev.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Assingment_AppDev.Controllers
{
    public class TraineesController : Controller
    {
        private ApplicationDbContext _context;
        public TraineesController()
        {
            _context = new ApplicationDbContext();
        }
        [HttpGet]
        public ActionResult Index(string searchString)
        {
            var trainee = _context.Trainees.Include(te => te.Trainees);
            if (!String.IsNullOrEmpty(searchString))
            {
                trainee = trainee.Where(s => s.Trainees.UserName.Contains(searchString));
                return View(trainee);
            }

            if (User.IsInRole("TrainingStaff"))
            {
                var viewTrainee = _context.Trainees.Include(a => a.Trainees).ToList();
                return View(viewTrainee);
            }
            if (User.IsInRole("Trainee"))
            {
                var traineeId = User.Identity.GetUserId();
                var traineeVM = _context.Trainees.Where(te => te.TraineeID == traineeId).ToList();
                return View(traineeVM);
            }
            return View("Index");
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var trainee = _context.Trainees.SingleOrDefault(t => t.Id == id);
            if (trainee == null)
            {
                return HttpNotFound();
            }

            return View(trainee);
        }
        [HttpGet]
        public ActionResult Create()
        {
            //Get Account Trainee
            var userInDb = (from r in _context.Roles where r.Name.Contains("Trainee") select r).FirstOrDefault();
            var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(userInDb.Id)).ToList();
            var traineeUser = _context.Trainees.ToList();
            var viewModel = new TraineeViewModel
            {
                Trainees = users,
                Trainee = new Trainee()
            };
            return View(viewModel);
        }
            [HttpPost]
        public ActionResult Create(TraineeViewModel trainee)
        {
            var traineeinDb = (from te in _context.Roles where te.Name.Contains("Trainee") select te).FirstOrDefault();
            var traineeUser = _context.Users.Where(u => u.Roles.Select(us => us.RoleId).Contains(traineeinDb.Id)).ToList();
            if (ModelState.IsValid)
            {

                var checkTraineeExist = _context.Trainees.Include(t => t.Trainees).Where(t => t.Trainees.Id == trainee.Trainee.TraineeID);
                //GET TraineeID 
                if (checkTraineeExist.Count() > 0)  //list ID comparison, if count == 0. jump to else
                                                    // if (checkTraineeExist.Any())
                {
                    ModelState.AddModelError("", "Trainee Already Exists.");
                }
                else
                {
                    _context.Trainees.Add(trainee.Trainee);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            TraineeViewModel traineeUserView = new TraineeViewModel()
            {
                Trainees = traineeUser,
                Trainee = trainee.Trainee
            };
            return View(traineeUserView);
  
    }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var todo = _context.Trainees.SingleOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return HttpNotFound();
            }

            _context.Trainees.Remove(todo);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var trainee = _context.Trainees.SingleOrDefault(t => t.Id == id);
            if (trainee == null)
            {
                return HttpNotFound();
            }

            return View(trainee);
        }
        [HttpPost]
        public ActionResult Edit(Trainee trainee)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var traineeInDb = _context.Trainees.SingleOrDefault(te => te.Id == trainee.Id);
            if (traineeInDb == null)
            {
                return HttpNotFound();
            }
            traineeInDb.Email = trainee.Email;
            traineeInDb.Name = trainee.Name;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
