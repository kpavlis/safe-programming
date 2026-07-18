using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace academic_assignment_and_recording_app.Models;

[Table("student")]
public partial class Student
{
    [Key]
    [Column("student_id")]
    public int StudentId { get; set; }

    [Column("banned")]
    public bool? Banned { get; set; }

    [ForeignKey("StudentId")]
    [InverseProperty("Student")]
    public virtual User StudentNavigation { get; set; } = null!;

    [InverseProperty("Student")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    [ForeignKey("StudentId")]
    [InverseProperty("Students")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
