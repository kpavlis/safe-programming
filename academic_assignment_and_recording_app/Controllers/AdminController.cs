using academic_assignment_and_recording_app.Models;
using academic_assignment_and_recording_app.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace academic_assignment_and_recording_app.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly LabDBContext _context;

        public AdminController(ILogger<AdminController> logger, LabDBContext context) 
        {
            _logger = logger;
            _context = context;
        }


        public async Task<IActionResult> BanMethod(int? id,string role)
        {
        
            if (role.Equals("teacher"))
            {
                var teacher = _context.Teachers.FirstOrDefault(t => t.TeacherId == id);
                if (teacher != null)
                {
                    teacher.Banned = !teacher.Banned;
                }

                await _context.SaveChangesAsync();

            }
            else if (role.Equals("student"))
            {
                var student = _context.Students.FirstOrDefault(s => s.StudentId == id);
                if (student != null)
                {
                    student.Banned = !student.Banned;
                }

                await _context.SaveChangesAsync();
            }


            return RedirectToAction("Index","Admin");
        }


        public IActionResult Index()
        {
            ViewBag.FirstName = HttpContext.Session.GetString("userName");

            var teachers = _context.Teachers.ToList();
            var students = _context.Students.ToList();

            var users = _context.Users.Where(u => u.AcademicRole.Equals("teacher") || u.AcademicRole.Equals("student"));


            var viewModel = new TeachersAndStudentsViewModel
            {
                Teachers = teachers,
                Students = students,
                Users = users
            };

            ViewBag.UserName = HttpContext.Session.GetString("userName");
            ViewBag.UserSurname = HttpContext.Session.GetString("userSurname");

            return View(viewModel);

           
        }
    }
}
