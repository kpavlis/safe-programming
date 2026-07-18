using academic_assignment_and_recording_app.Models;

namespace academic_assignment_and_recording_app.ViewModels
{
    public class TeachersAndStudentsViewModel
    {
        public IEnumerable<Teacher> Teachers { get; set; }
        public IEnumerable<Student> Students { get; set; }

        public IEnumerable<User> Users { get; set; }
    }
}
