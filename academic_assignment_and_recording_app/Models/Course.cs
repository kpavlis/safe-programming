using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace academic_assignment_and_recording_app.Models;

[Table("course")]
public partial class Course
{
    [Column("title")]
    public string? Title { get; set; }

    [Key]
    [Column("course_id")]
    public int CourseId { get; set; }

    [InverseProperty("Course")]
    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();

    [ForeignKey("CourseId")]
    [InverseProperty("Courses")]
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    [ForeignKey("CourseId")]
    [InverseProperty("Courses")]
    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
