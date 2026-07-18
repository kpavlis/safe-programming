using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace academic_assignment_and_recording_app.Models;

[Table("teacher")]
public partial class Teacher
{
    [Key]
    [Column("teacher_id")]
    public int TeacherId { get; set; }

    [Column("banned")]
    public bool? Banned { get; set; }

    [ForeignKey("TeacherId")]
    [InverseProperty("Teacher")]
    public virtual User TeacherNavigation { get; set; } = null!;

    [ForeignKey("TeacherId")]
    [InverseProperty("Teachers")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
