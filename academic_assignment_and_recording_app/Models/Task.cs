using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace academic_assignment_and_recording_app.Models;

[Table("task")]
public partial class Task
{
    [Column("task_title")]
    public string? TaskTitle { get; set; }

    [Column("group_type")]
    public bool? GroupType { get; set; }

    [Column("comments")]
    public string? Comments { get; set; }

    [Column("subject_id")]
    public int? SubjectId { get; set; }

    [Key]
    [Column("task_id")]
    public int TaskId { get; set; }

    [Column("student_id")]
    public int? StudentId { get; set; }

    [ForeignKey("StudentId")]
    [InverseProperty("Tasks")]
    public virtual Student? Student { get; set; }

    [ForeignKey("SubjectId")]
    [InverseProperty("Tasks")]
    public virtual Subject? Subject { get; set; }
}
