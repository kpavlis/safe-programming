using academic_assignment_and_recording_app.Models;

namespace academic_assignment_and_recording_app.ViewModels
{
    public class TaskListAndSubjectViewModel
    {
        public IEnumerable<Models.Task> tasks { get; set; }

        public Subject subject { get; set; }

        public bool has_access { get; set; }
    }
}
