using academic_assignment_and_recording_app.Models;
using academic_assignment_and_recording_app.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace academic_assignment_and_recording_app.Controllers
{
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private readonly LabDBContext _context;

        public StudentController(ILogger<StudentController> logger, LabDBContext context)
        {
            _logger = logger;
            _context = context;
        }


        public IActionResult ViewStudentSubject(int? id)
        {
            if (id != null)
            {
                var subjects = _context.Subjects.Where(s => s.CourseId == id).ToList();

                return View(
                    new SubjectListAndCourseIdViewModel
                    {
                        subjects = subjects,
                        courseId = (int)id,
                        has_access = false
                    }
                    );
            }
            else
            {
                return NotFound();
            }
        }


        public IActionResult ViewStudentTask(int id)
        {
            //ViewBag.taskHasExpired = false;

            var tasks = _context.Tasks.Where(t => t.SubjectId == id).ToList();

            var subject = _context.Subjects.FirstOrDefault(t => t.SubjectId == id);

            bool has_not_access = _context.Tasks.Any(t => t.SubjectId == id && t.StudentId == Int32.Parse(HttpContext.Session.GetString("userId")));

            if (has_not_access) 
            {
                ViewBag.studentTask = _context.Tasks.FirstOrDefault(t => t.SubjectId == id && t.StudentId == Int32.Parse(HttpContext.Session.GetString("userId")));
            }
            else
            {
                ViewBag.studentTask = null;
            }
                
            
            ViewBag.task_is_available = _context.Subjects.Any(s => s.SubjectId == id && s.SubjectDeadline > DateTime.Now);
            //ViewBag.taskHasExpired = taskHasExpiredValue;


            return View(
                    new TaskListAndSubjectViewModel
                    {
                        tasks = tasks,
                        subject = subject,
                        has_access = !has_not_access
                    }
                    );
        }

        [HttpPost]
        public async Task<IActionResult> SaveTask(int taskId,string comments,int subjectId)
        {
            var task = _context.Tasks.FirstOrDefault(
                    task => 
                        task.TaskId == taskId && task.StudentId == null && string.IsNullOrEmpty(task.Comments));

            if (task != null)
            {
                //var dateTimeNow = DateTime.Now;

                var subject = _context.Subjects.FirstOrDefault(t => t.SubjectId == task.SubjectId);

                if (subject != null) 
                {
                    if(DateTime.Now <= subject.SubjectDeadline)
                    {
                        task.StudentId = Int32.Parse(HttpContext.Session.GetString("userId"));

                        if (string.IsNullOrEmpty(comments))
                        {
                            comments = "Κανένα Σχόλιο";
                        }
                        task.Comments = comments;

                        _context.Update(task);

                        await _context.SaveChangesAsync();

                        //return RedirectToAction("Index");
                    }
                }
            }
            return RedirectToAction("ViewStudentTask", new { id = subjectId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTask(int taskId,int subjectId)
        {
            var task = _context.Tasks.FirstOrDefault(task => task.TaskId == taskId);

            if (task != null)
            {
                if(task.StudentId != null && !string.IsNullOrEmpty(task.Comments))
                {
                    task.StudentId = null;
                    task.Comments = null;

                    _context.Update(task);

                    await _context.SaveChangesAsync();
                }
                

                //return RedirectToAction("Index");
            }

            return RedirectToAction("ViewStudentTask", new { id = subjectId });
        }


        public IActionResult Index()
        {
            var coursesForStudent = _context.Courses.Where(c => c.Students.Any(s => s.StudentId == Int32.Parse(HttpContext.Session.GetString("userId")))).ToList();

            ViewBag.UserName = HttpContext.Session.GetString("userName");
            ViewBag.UserSurname = HttpContext.Session.GetString("userSurname");

            return View(coursesForStudent);
        }
    }
}
