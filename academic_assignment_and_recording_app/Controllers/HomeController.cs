using academic_assignment_and_recording_app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace academic_assignment_and_recording_app.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LabDBContext _context;

        public HomeController(ILogger<HomeController> logger, LabDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult LogIn(string email,string password) { 
            var user = _context.Users.FirstOrDefault(x => x.Email == email);

            if (user != null) {

                if (user.Password == password)
                {
                    if (user.AcademicRole.Equals("admin"))
                    {

                        HttpContext.Session.SetString("userId", user.UserId.ToString());
                        HttpContext.Session.SetString("userName", user.Name);
                        HttpContext.Session.SetString("userSurname", user.Surname);
                        HttpContext.Session.SetString("userRole", user.AcademicRole);
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (user.AcademicRole.Equals("teacher"))
                    {
                        var teacher = _context.Teachers.FirstOrDefault(u => u.TeacherId == user.UserId);

                        if (teacher != null)
                        {
                            if (teacher.Banned!=null)
                            {
                                if (teacher.Banned == false)
                                {

                                    HttpContext.Session.SetString("userId", user.UserId.ToString());
                                    HttpContext.Session.SetString("userName", user.Name);
                                    HttpContext.Session.SetString("userSurname", user.Surname);
                                    HttpContext.Session.SetString("userRole", user.AcademicRole);
                                    return RedirectToAction("Index", "Teacher");
                                }
                                else
                                {
                                    return View("LogInError");
                                }
                                
                            }
                        }
                        
                    }
                    else
                    {
                        var students = _context.Students.FirstOrDefault(u => u.StudentId == user.UserId);

                        if (students != null)
                        {
                            if (students.Banned != null)
                            {
                                if (students.Banned == false)
                                {

                                    HttpContext.Session.SetString("userId", user.UserId.ToString());
                                    HttpContext.Session.SetString("userName", user.Name);
                                    HttpContext.Session.SetString("userSurname", user.Surname);
                                    HttpContext.Session.SetString("userRole", user.AcademicRole);
                                    return RedirectToAction("Index", "Student");
                                }
                                else
                                {
                                    return View("LogInError");
                                }
                              
                            }
                        }
                    }
                }
            
            }

            return View("LogInAgain");
        }

        // New methods for Testing SQL Injection vulnerability

        public IActionResult LogInAgain()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TestVulnerability(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = "1";
            }

            try
            {
                string query = "SELECT * FROM public.\"user\" WHERE user_id = " + id;

                var result = _context.Users.FromSqlRaw(query).ToList();

                if (result.Count > 0)
                {
                    return Ok("TRUE_RESPONSE: Data found for this ID.");
                }
                else
                {
                    return Ok("FALSE_RESPONSE: No data found.");
                }
            }
            catch (Exception ex)
            {
                string dbError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, dbError);
            }
        }

        // End of new methods for Testing SQL Injection vulnerability

        public IActionResult Signout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
