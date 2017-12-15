using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using loginregister.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;



namespace loginregister.Controllers
{
    public class HomeController : Controller
    {

        private MainContext _context;

        public HomeController(MainContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            // LoginUser newuser = new LoginUser
            // {
            //     LogEmail = "habiba@habiba.com",
            //     LogPassword = "password",
            //     ConfirmLogPassword = "password",
            // };
            // return Login(newuser);
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // System.Console.WriteLine(registeredcheck);
                // System.Console.WriteLine("EMAILLL" + user.Email);
                // System.Console.WriteLine("THISSSSS****"+returnedid.id);

                User registeredcheck = _context.users.SingleOrDefault(str => str.Email == user.Email);


                // System.Console.WriteLine("THE EMAILLL", registeredcheck.Email);
                // string email = registeredcheck.Email;
                if (registeredcheck == null)
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    user.Password = Hasher.HashPassword(user, user.Password);
                    User NewPerson = new User

                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Password = user.Password,

                    };

                    _context.Add(NewPerson);
                    _context.SaveChanges();
                    System.Console.WriteLine("NEW PERSON", NewPerson.FirstName);
                    ViewBag.Success = "You have been added to the database! Please log in now!";
                    return View("Index");

                }
                else
                {
                    System.Console.WriteLine("ALREADY IN THE DATABASE");
                    return View("Index");
                }
            }
            return View("Index");
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginUser user)
        {
            User userfound = new User
            {
                FirstName = "john",
                LastName = "doe",
                Email = user.LogEmail,
                Password = user.LogPassword,
                ConfirmPassword = user.ConfirmLogPassword
            };

            User loggeduser = _context.users.SingleOrDefault(str => str.Email == userfound.Email);
            if (loggeduser == null)
            {
                ViewBag.loginerror = "Login failed, email and password did not match the information in the database. If you haven't registered please register first!";
                return View("Index");
            }
            else
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();

                if (0 != Hasher.VerifyHashedPassword(loggeduser, loggeduser.Password, userfound.Password))
                {

                    HttpContext.Session.SetInt32("loggedperson", (int)loggeduser.UserId);
                    HttpContext.Session.SetString("loggedpersonname", loggeduser.FirstName);

                    return RedirectToAction("LandingPage");
                }
                else
                {

                    ViewBag.loginerror = "Login failed, email and password did not match the information in the database. If you haven't registered please register first!";
                    return View("Index");
                }
            }
        }

        [HttpGet]
        [Route("home")]

        public IActionResult LandingPage()
        {

            int? loggedperson = HttpContext.Session.GetInt32("loggedperson");
            if (loggedperson == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                System.Console.WriteLine("HEYYY" + loggedperson);
                User findtheperson = _context.users.SingleOrDefault(str => str.UserId == loggedperson);

                System.Console.WriteLine("FOUND PESON " + findtheperson);

                List<Activity> AllEvents = _context.activities
                    .Include(y => y.Participants)
                    .ToList();
                ViewBag.ShowAll = AllEvents;

                ViewBag.User = findtheperson;
                return View("About");
            }

        }

        [HttpGet]
        [Route("addactivity")]
        public IActionResult NewActivity()
        {
            int? loggedperson = HttpContext.Session.GetInt32("loggedperson");
            if (loggedperson == null)
            {
                return RedirectToAction("LandingPage");
            }
            else
            {
                return View("NewActivity");
            }
        }


        [HttpPost]
        [Route("CreateActivity")]
        public IActionResult CreateActivity(Activity activity)
        {
            int? loggedperson = HttpContext.Session.GetInt32("loggedperson");
            if (loggedperson == null)
            {
                return RedirectToAction("LandingPage");
            }
            else
            {

                if (ModelState.IsValid)
                {
                    Activity NewActivity = new Activity
                    {
                        Title = activity.Title,
                        ActivityDate = activity.ActivityDate,
                        ActivityTime = activity.ActivityTime,
                        Duration = activity.Duration,
                        DurationType = activity.DurationType,
                        Description = activity.Description,
                        CreatedById = (int)HttpContext.Session.GetInt32("loggedperson"),
                        CreatedByFirstName = (string)HttpContext.Session.GetString("loggedpersonname"),
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    };
                    _context.Add(NewActivity);
                    _context.SaveChanges();
                    return RedirectToAction("LandingPage");
                }
                else
                {
                    return View("NewActivity");
                }
            }

        }


        [HttpGet]
        [Route("showone/{ActivityId}")]

        public IActionResult ShowOne (int ActivityId)
        {
            int? loggedperson = HttpContext.Session.GetInt32("loggedperson");
            if (loggedperson == null)
            {
                return RedirectToAction("LandingPage");
            }
            else
            {


            Activity showone = _context.activities
                .Include(y => y.Participants).
                ThenInclude(f => f.UserDetail)
                .Where(x => x.ActivityId == ActivityId)
                .SingleOrDefault();

         
                
                

            @ViewBag.showone = showone;

            return View("ShowOne");
        
            }

        }



        [HttpGet]
        [Route("Participate/{ActivityId}")]
        public IActionResult Participate(int ActivityId)
        {
            int? loggedperson = HttpContext.Session.GetInt32("loggedperson");
            if (loggedperson == null)
            {
                return RedirectToAction("LandingPage");
            }
            else
            {


                    Participant NewParticipant = new Participant
                    {
                        UserId = (int)loggedperson,
                        ActivityId = ActivityId,

                    };
                    _context.Add(NewParticipant);
                    _context.SaveChanges();
                    return RedirectToAction("LandingPage");
        
            }

        }

        [HttpGet]
        [Route("Leave/{ActivityId}")]
        public IActionResult Leave(int ActivityId)
        {
            int? loggedperson = HttpContext.Session.GetInt32("loggedperson");
            if (loggedperson == null)
            {
                return RedirectToAction("LandingPage");
            }
            else
            {
                var leave = _context.participants
                            .Where( w => w.ActivityId == ActivityId)
                            .Where(g => g.UserId == loggedperson)
                            .SingleOrDefault();

                    _context.participants.Remove(leave);
                    _context.SaveChanges();
                    return RedirectToAction("LandingPage");
        
            }

        }

        [HttpGet]
        [Route("Delete/{ActId}")]
        public IActionResult Delete(int ActId)
        {
            int? loggedperson = HttpContext.Session.GetInt32("loggedperson");
            if (loggedperson == null)
            {
                return RedirectToAction("LandingPage");
            }
            else
            {
                var delete = _context.activities
                            .Include( w => w.Participants)
                            .ThenInclude ( g => g.UserDetail)
                            .Where (m => m.ActivityId == ActId)
                            .SingleOrDefault();

                    _context.activities.Remove(delete);
                    _context.SaveChanges();
                    return RedirectToAction("LandingPage");
        
            }

        }

        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {


            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout2()
        {


            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }






    }
}
