using academic_assignment_and_recording_app.Models;
namespace academic_assignment_and_recording_app.ViewModels
{
    public class TaskList_UserListAndSubjectViewModel
    {
        public IEnumerable<Models.Task> tasks { get; set; }

        public IEnumerable<Models.User> users { get; set; }

        public Subject subject { get; set; }

        public bool has_access { get; set; }
    }
}
