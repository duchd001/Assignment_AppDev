using Assignment_AppDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ASM_AppDevDD.Controllers
{
    public class TraineesController : Controller
    {
        private ApplicationDbContext _context;
        public TraineesController()
        {
            _context = new ApplicationDbContext();
        }
        [HttpGet]
        public ActionResult Index()
        {
            var trainees = _context.Trainees.ToList();
            return View(trainees);
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var trainee = _context.Trainees.SingleOrDefault(t => t.TraineeID == id);
            if (trainee == null)
            {
                return HttpNotFound();
            }

            return View(trainee);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Trainee model)
        {
            var newTrainee = new Trainee()
            {
                TraineeID = model.TraineeID,
                Name = model.Name,
                Email = model.Email,
            };

            _context.Trainees.Add(newTrainee);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var todo = _context.Trainees.SingleOrDefault(t => t.TraineeID == id);
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
            var trainee = _context.Trainees.SingleOrDefault(t => t.TraineeID == id);
            if (trainee == null)
            {
                return HttpNotFound();
            }

            return View(trainee);
        }
        [HttpPost]
        public ActionResult Edit(Trainee model)
        {
            var traineeDb = _context.Trainees.SingleOrDefault(t => t.TraineeID == model.TraineeID);
            if (traineeDb == null)
            {
                return HttpNotFound();
            }

            traineeDb.TraineeID = model.TraineeID;
            traineeDb.Name = model.Name;
            traineeDb.Email = model.Email;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
