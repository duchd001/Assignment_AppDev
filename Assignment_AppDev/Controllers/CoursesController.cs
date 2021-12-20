using Assignment_AppDev.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Assignment_AppDev.Controllers
{
	[Authorize(Roles = "TrainingStaff")]
	public class CoursesController : Controller
    {
        private ApplicationDbContext _context;
        public CoursesController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Courses
        public ActionResult Index(string searchString)
        {
			var course = _context.Courses.ToList();

			if (!String.IsNullOrEmpty(searchString))
			{
				course = course.FindAll(s => s.Name.Contains(searchString));
			}

			return View(course);
		}
		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Create(Category category)
		{
			if (!ModelState.IsValid)
			{
				return RedirectToAction("Create");
			}
			var check = _context.Categories.Any(
				c => c.Name.Contains(category.Name));
			if (check)
			{
				ModelState.AddModelError("", "Category Already Exists.");
				return View("Create");
			}
			_context.Categories.Add(category);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
		[HttpGet]
		public ActionResult Edit(int id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Category category = _context.Categories.Find(id);
			if (category == null)
			{
				return HttpNotFound();
			}
			return View();
		}
		[HttpPost]
		public ActionResult Edit(Category category)
		{

			if (ModelState.IsValid)
			{
				_context.Entry(category).State = EntityState.Modified;
				_context.SaveChanges();
				return RedirectToAction("Index");
			}
			var check = _context.Categories.Any(
				c => c.Name.Contains(category.Name));

			if (check)
			{
				ModelState.AddModelError("", "Category Already Exists.");
				return View("Index");
			}
			category.Name = category.Name;
			category.Description = category.Description;
			return View(category);
		}
		[HttpGet]
		public ActionResult Delete(int id)
		{
			var categoryInDb = _context.Categories.SingleOrDefault(c => c.ID == id);

			if (categoryInDb == null)
			{
				return HttpNotFound();
			}

			_context.Categories.Remove(categoryInDb);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}