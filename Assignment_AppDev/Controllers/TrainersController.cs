using Assignment_AppDev.Models;
using System;
using System.Collections.Generic;
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
        public ActionResult Index()
        {
            var trainers = _context.Trainers.ToList();
            return View(trainers);
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var trainer = _context.Trainers.SingleOrDefault(t => t.TrainerID == id);
            if (trainer == null)
            {
                return HttpNotFound();
            }

            return View(trainer);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Trainer model)
        {
            var newTrainer = new Trainer()
            {
                TrainerID = model.TrainerID,
                Name = model.Name,
                Address = model.Address,
                Phone = model.Phone,
                Age = model.Age,
            };

            _context.Trainers.Add(newTrainer);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var todo = _context.Trainers.SingleOrDefault(t => t.TrainerID == id);
            if (todo == null)
            {
                return HttpNotFound();
            }

            _context.Trainers.Remove(todo);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var trainer = _context.Trainers.SingleOrDefault(t => t.TrainerID == id);
            if (trainer == null)
            {
                return HttpNotFound();
            }

            return View(trainer);
        }
        [HttpPost]
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