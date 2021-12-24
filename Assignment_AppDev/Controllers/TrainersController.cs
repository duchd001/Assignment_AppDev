using Assignment_AppDev.Models;
using Assignment_AppDev.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASM_AppDevDD.Controllers
{
    public class TrainersController : Controller
    {
        // GET: Trainers
        private ApplicationDbContext _context;
        public TrainersController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        [Authorize(Roles = "TrainingStaff, Trainer")]
        public ActionResult Index()
        {
            if (User.IsInRole("TrainingStaff"))
            {
                var viewTrainer = _context.Trainers.Include(a => a.Trainers).ToList();
                return View(viewTrainer);
            }
            if (User.IsInRole("Trainer"))
            {
                var trainerId = User.Identity.GetUserId();
                var trainerVM = _context.Trainers.Where(te => te.TrainerID == trainerId).ToList();
                return View(trainerVM);
            }
            return View("Index");
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var trainer = _context.Trainers.SingleOrDefault(t => t.ID == id);
            if (trainer == null)
            {
                return HttpNotFound();
            }

            return View(trainer);
        }
        [HttpGet]
        [Authorize(Roles = "TrainingStaff")]
        public ActionResult Create()
        {
            //Get Account Trainer
            var userInDb = (from r in _context.Roles where r.Name.Contains("Trainer") select r).FirstOrDefault();
            var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(userInDb.Id)).ToList();
            var trainerUser = _context.Trainers.ToList();

            var viewModel = new TrainerViewModel
            {
                Trainers = users,
                Trainer = new Trainer()
            };
            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "TrainingStaff")]
        public ActionResult Create(TrainerViewModel trainer)
        {
            var trainerinDb = (from te in _context.Roles where te.Name.Contains("Trainer") select te).FirstOrDefault();
            var trainerUser = _context.Users.Where(u => u.Roles.Select(us => us.RoleId).Contains(trainerinDb.Id)).ToList();
            try
            {
                _context.Trainers.Add(trainer.Trainer);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                TrainerViewModel trainerUserView = new TrainerViewModel()
                {
                    Trainers = trainerUser,
                    Trainer = trainer.Trainer
                };

                return View(trainerUserView);
            }
            
        }
        [HttpGet]
        [Authorize(Roles = "TrainingStaff")]
        public ActionResult Delete(int id)
        {
            var todo = _context.Trainers.SingleOrDefault(t => t.ID == id);
            if (todo == null)
            {
                return HttpNotFound();
            }

            _context.Trainers.Remove(todo);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Roles = "TrainingStaff, Trainer")]
        public ActionResult Edit(int id)
        {
            var trainer = _context.Trainers.SingleOrDefault(t => t.ID == id);
            if (trainer == null)
            {
                return HttpNotFound();
            }

            return View(trainer);
        }
        [HttpPost]
        [Authorize(Roles = "TrainingStaff, Trainer")]
        public ActionResult Edit(Trainer model)
        {
            var trainerDb = _context.Trainers.SingleOrDefault(t => t.TrainerID == model.TrainerID);
            if (trainerDb == null)
            {
                return HttpNotFound();
            }

            trainerDb.TrainerID = model.TrainerID;
            trainerDb.Name = model.Name;
            trainerDb.Age = model.Age;
            trainerDb.Address = model.Address;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}