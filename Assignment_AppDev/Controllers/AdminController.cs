using Assignment_AppDev.Models;
using Assignment_AppDev.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Assignment_AppDev.Controllers
{
    [Authorize(Roles = Role.Admin)]
    public class AdminController : Controller
    {
        // GET: Admin
        private ApplicationDbContext _context;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AdminController()
        {
            _context = new ApplicationDbContext();  
        }

        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CreateTrainerAccount()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateTrainerAccount(RegisterViewModel model)
        {
            //Borrow from AccountController
            if (ModelState.IsValid)
            {
                var trainerInf = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await UserManager.CreateAsync(trainerInf, model.Password);
                if (result.Succeeded)
                {
                    //Add to trainer role
                    await UserManager.AddToRoleAsync(trainerInf.Id, Role.Trainer);
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult CreateStaffAccount()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateStaffAccount(RegisterViewModel model)
        {
            //Borrow from AccountController
            if (ModelState.IsValid)
            {
                var staffInf = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await UserManager.CreateAsync(staffInf, model.Password);
                if (result.Succeeded)
                {
                    //Add to training staff role
                    await UserManager.AddToRoleAsync(staffInf.Id, Role.Trainer);
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult ShowTrainersInfo()
        {
            var role = _context.Roles
              .SingleOrDefault(r => r.Name.Equals(Role.Trainer));
            var users = _context.Users
              .Where(m => m.Roles.Any(r => r.RoleId.Equals(role.Id)))
              .ToList();
            return View("ShowInfo", users);
        }
        [HttpGet]
        public ActionResult ShowStaffsInfo()
        {
            var role = _context.Roles
              .SingleOrDefault(r => r.Name.Equals(Role.TrainingStaff));
            var users = _context.Users
              .Where(m => m.Roles.Any(r => r.RoleId.Equals(role.Id)))
              .ToList();
            return View("ShowInfo", users);
        }

        [HttpGet]
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
        public ActionResult Edit(ApplicationUser user)
        {
            var userInDb = _context.Users.Find(user.Id);

            if (userInDb == null)
            {
                return View(user);
            }

            if (ModelState.IsValid)
            {

                userInDb.Email = user.Email;
                _context.Users.AddOrUpdate(userInDb);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(user);

        }
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
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

    }
}