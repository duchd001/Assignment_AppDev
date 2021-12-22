using Assignment_AppDev.Models;
using Assignment_AppDev.Utils;
using Assignment_AppDev.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Assignment_AppDev.Controllers
{
	public class CoursesController : Controller
    {
        private ApplicationDbContext _context;
        public CoursesController()
        {
            _context = new ApplicationDbContext();
        }
		// GET: Courses
		[HttpGet]
		public ActionResult Index(string searchString)
        {
			var courses = _context.Courses.Include(c => c.Category);

			if (!String.IsNullOrEmpty(searchString))
			{
						courses = courses.Where(
						s => s.Name.Contains(searchString) ||
						s.Category.Name.Contains(searchString));
			}

			return View(courses);
		}
		[HttpGet]
		public ActionResult Create()
		{
			var viewModel = new CourseCategoryViewModel
			{
				Categories = _context.Categories.ToList(),
				Course = new Course(),
			};
			return View(viewModel);
		}

		[HttpPost]
		public ActionResult Create(CourseCategoryViewModel coursecate)
		{
			if (ModelState.IsValid)
			{
				var check = _context.Courses.Include(c => c.Category)
					.Where(c => c.Name == coursecate.Course.Name && c.CategoryID == coursecate.Course.CategoryID);
				//GET NameCOurse and Category ID from VM
				if (check.Count() > 0) //list ID comparison, if count == 0. jump to else
				{
					ModelState.AddModelError("", "Course Already Exists.");
				}
				else
				{
					_context.Courses.Add(coursecate.Course);
					_context.SaveChanges();
					return RedirectToAction("Index");
				}
			}
			var courseVM = new CourseCategoryViewModel()
			{
				Categories = _context.Categories.ToList(),
				Course = coursecate.Course,
			};
			return View(courseVM);
		}
		[HttpGet]
		public ActionResult Edit(int id)
		{
			var courseInDb = _context.Courses.SingleOrDefault(c => c.ID == id);
			if (courseInDb == null)
			{
				return HttpNotFound();
			}
			var viewModel = new CourseCategoryViewModel
			{
				Course = courseInDb,
				Categories = _context.Categories.ToList()
			};
			return View(viewModel);
		}
		[HttpPost]
		public ActionResult Edit(CourseCategoryViewModel edit)
		{
			if (ModelState.IsValid)
			{
				var check = _context.Courses.Include(c => c.Category).Where(c => c.Name == edit.Course.Name && c.CategoryID == edit.Course.CategoryID);

				if (check.Count() > 0)
				{
					ModelState.AddModelError("", "Course Already Exists.");
				}
				else
				{
					var courseInDb = _context.Courses.Find(edit.Course.ID);
					courseInDb.Name = edit.Course.Name;
					courseInDb.Description = edit.Course.Description;
					_context.SaveChanges();
					return RedirectToAction("Index");
				}
			}

			var courseVM = new CourseCategoryViewModel()
			{
				Categories = _context.Categories.ToList(),
				Course = edit.Course,
			};
			return View(courseVM);
		}
		[HttpGet]
		public ActionResult Delete(int id)
		{
			var courseInDb = _context.Courses.SingleOrDefault(c => c.ID == id);

			if (courseInDb == null)
			{
				return HttpNotFound();
			}
			_context.Courses.Remove(courseInDb);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}