using Assignment_AppDev.Models;
using Assignment_AppDev.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
        //CreateTrainerAccount
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
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
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
        //CreateStaffAccount
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
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };
                var result = await UserManager.CreateAsync(staffInf, model.Password);
                if (result.Succeeded)
                {
                    //Add to training staff role
                    await UserManager.AddToRoleAsync(staffInf.Id, Role.TrainingStaff);
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }
            return View(model);
        }
        //Show Trainer Info
        [HttpGet]
        public ActionResult ShowTrainersInfo()
        {
            var role = _context.Roles
              .SingleOrDefault(r => r.Name.Equals(Role.Trainer));
            var users = _context.Users
              .Where(m => m.Roles.Any(r => r.RoleId.Equals(role.Id)))
              .ToList();
            return View("ShowTrainersInfo", users);
        }
        //Show Staff Info
        [HttpGet]
        public ActionResult ShowStaffsInfo()
        {
            var role = _context.Roles
              .SingleOrDefault(r => r.Name.Equals(Role.TrainingStaff));
            var users = _context.Users
              .Where(m => m.Roles.Any(r => r.RoleId.Equals(role.Id)))
              .ToList();
            return View("ShowStaffsInfo", users);
        }
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public ActionResult EditTrainer(string id)
        {
            // Find and assign Id value in the Trainer table to userInDb
            var userInDb = _context.Users.SingleOrDefault(u => u.Id == id);
            if (userInDb == null)
            {
                return HttpNotFound();
            }

            return View(userInDb);
        }
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public ActionResult EditTrainer(ApplicationUser user)
        {
            // Check the value of Id
            if (!ModelState.IsValid)
            {
                return View();
            }
            var usernameExist = _context.Users.Any(u => u.UserName.Contains(user.UserName));
            if (usernameExist)
            {
                ModelState.AddModelError("UserName", "Username existed");
                return View();
            }
            var userInDb = _context.Users.SingleOrDefault(u => u.Id == user.Id);

            if (userInDb == null)
            {
                return HttpNotFound();
            }
            userInDb.UserName = user.UserName;
            userInDb.Email = user.Email;
            _context.SaveChanges();

            return RedirectToAction("ShowTrainersInfo");
        }

        public ActionResult EditStaff(string id)
        {
            // Find and assign Id value in the Staff table to userInDb
            var userInDb = _context.Users.SingleOrDefault(u => u.Id == id);
            if (userInDb == null)
            {
                return HttpNotFound();
            }

            return View(userInDb);
        }
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public ActionResult EditStaff(ApplicationUser user)
        {
            // Check the value of Id
            if (!ModelState.IsValid)
            {
                return View();
            }
            var usernameExist = _context.Users.Any(u => u.UserName.Contains(user.UserName));
            //  var EmailIsExist = _context.Users.Any(u => u.Email.Contains(user.Email));
            if (usernameExist)
            {
                ModelState.AddModelError("UserName", "Username existed");
                return View();
            }
            var userInDb = _context.Users.SingleOrDefault(u => u.Id == user.Id);

            if (userInDb == null)
            {
                return HttpNotFound();
            }
            userInDb.UserName = user.UserName;
            userInDb.Email = user.Email;
            _context.SaveChanges();

            return RedirectToAction("ShowStaffsInfo");
        }
        public ActionResult DeleteTrainer(string id)
        {
            var userInDb = _context.Users.SingleOrDefault(p => p.Id == id);

            if (userInDb == null)
            {
                return HttpNotFound();
            }
            _context.Users.Remove(userInDb);
            _context.SaveChanges();
            return RedirectToAction("ShowTrainersInfo");
        }
        public ActionResult DeleteStaff(string id)
        {
            var userInDb = _context.Users.SingleOrDefault(p => p.Id == id);

            if (userInDb == null)
            {
                return HttpNotFound();
            }
            _context.Users.Remove(userInDb);
            _context.SaveChanges();
            return RedirectToAction("ShowStaffsInfo");
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