using academic_assignment_and_recording_app.Models;
namespace academic_assignment_and_recording_app.ViewModels
{
    public class SubjectListAndCourseIdViewModel
    {
        public IEnumerable<Subject> subjects { get; set; }
        public int courseId { get; set; }
        public bool has_access {  get; set; }
    }
}
