using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace academic_assignment_and_recording_app.Models;

[Table("subject")]
public partial class Subject
{
    [Column("subject_title")]
    public string SubjectTitle { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("course_id")]
    public int? CourseId { get; set; }

    [Key]
    [Column("subject_id")]
    public int SubjectId { get; set; }

    [Column("subjectDeadline", TypeName = "timestamp without time zone")]
    public DateTime? SubjectDeadline { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("Subjects")]
    public virtual Course? Course { get; set; }

    [InverseProperty("Subject")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
