using academic_assignment_and_recording_app.Models;
using academic_assignment_and_recording_app.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace academic_assignment_and_recording_app.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ILogger<TeacherController> _logger;
        private readonly LabDBContext _context;

        public TeacherController(ILogger<TeacherController> logger, LabDBContext context) 
        { 
            _logger = logger;
            _context = context;
        }

        public IActionResult ViewTeacherSubject(int? id)
        {
            if (id != null)
            {
                var subjects = _context.Subjects.Where(s => s.CourseId == id).ToList();

                bool has_access = Teacher_access((int)id);

                return View(
                    new SubjectListAndCourseIdViewModel
                    {
                        subjects = subjects,
                        courseId = (int)id,
                        has_access = has_access
                    }
                );
            }
            else
            {
                return NotFound();
            }
        }


        public IActionResult ViewTeacherTask(int id)
        {
            var tasks = _context.Tasks.Where(t => t.SubjectId == id).ToList();

            var users = _context.Users.Where(s => s.AcademicRole.Equals("student"));

            bool has_access = false;

            var subject = _context.Subjects.FirstOrDefault(s => s.SubjectId == id);

            if (subject != null)
            {
                has_access = Teacher_access((int)subject.CourseId);


                return View(
                    new TaskList_UserListAndSubjectViewModel
                    {
                        tasks = tasks,
                        users = users,
                        subject = subject,
                        has_access = has_access
                    }
                );
            }

            return NotFound();
        }


        public IActionResult CreateSubject(int? id)
        {
            ViewBag.CourseId = id;
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> CreateSubject(string title,string description,DateTime dateTime,int courseId)
        {

            try
            {
                _context.Add(

                    new Subject
                    {
                        SubjectTitle = title,
                        Description = description,
                        CourseId = courseId,
                        SubjectDeadline = dateTime
                    }
                    
                    
                    );
                await _context.SaveChangesAsync();
                return RedirectToAction("ViewTeacherSubject", new { id = courseId });
            }
            catch (DbUpdateException ex)
            {
                return RedirectToAction("CreateSubject");
            }
        }


        [HttpPost]
        public async Task<IActionResult> DeleteSubject(int subjectId,int courseId)
        {
            var subject = _context.Subjects.Include(s => s.Tasks).FirstOrDefault(s => s.SubjectId == subjectId);

            if (subject != null)
            {
                _context.Subjects.Remove(subject);

                await _context.SaveChangesAsync();

                return RedirectToAction("ViewTeacherSubject", new { id = courseId });
            }

            return RedirectToAction("Index");

            
        }


        public IActionResult CreateTask(int? id)
        {
            ViewBag.SubjectId = id;
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> CreateTask(string title,bool? group_type,int subjectId)
        {
            if (group_type==null) 
            { 
                group_type = false;
            }

            try
            {
                _context.Add(

                    new Models.Task
                    {
                        TaskTitle = title,
                        GroupType = group_type,
                        SubjectId = subjectId
                    }
                    
                    );
                await _context.SaveChangesAsync();
                return RedirectToAction("ViewTeacherTask", new { id = subjectId });
            }
            catch (DbUpdateException ex)
            {
                return RedirectToAction("ViewTeacherTask", new { id = subjectId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTask(int taskId, int subjectId)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.TaskId == taskId && t.StudentId == null && string.IsNullOrEmpty(t.Comments));

            if (task != null)
            {

                _context.Remove(task);
                await _context.SaveChangesAsync();
                return RedirectToAction("ViewTeacherTask", new { id = subjectId });
            }

            return RedirectToAction("ViewTeacherTask", new { id = subjectId });

        }



        public IActionResult ViewOtherCourses()
        {
            var coursesForStudent = _context.Courses.Where(c => c.Teachers.Any(t => t.TeacherId != Int32.Parse(HttpContext.Session.GetString("userId")))).ToList();

            return View("Index",coursesForStudent);
        }


        public IActionResult Index()
        {

            var coursesForStudent = _context.Courses.Where(c => c.Teachers.Any(t => t.TeacherId == Int32.Parse(HttpContext.Session.GetString("userId")))).ToList();

            ViewBag.UserName = HttpContext.Session.GetString("userName");
            ViewBag.UserSurname = HttpContext.Session.GetString("userSurname");


            return View(coursesForStudent);
        }

        public bool Teacher_access(int id)
        {
            var course = _context.Courses.Include(c =>c.Teachers).FirstOrDefault(s => s.CourseId == id);

            bool has_access = false;

            if (course != null)
            {
                if (course.Teachers.Any(t => t.TeacherId == Int32.Parse(HttpContext.Session.GetString("userId"))))
                {
                    has_access = true;
                }
            }

            return has_access;
        }
    }
}
