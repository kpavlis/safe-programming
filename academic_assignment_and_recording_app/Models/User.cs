using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace academic_assignment_and_recording_app.Models;

[Table("user")]
public partial class User
{
    [Column("name")]
    public string? Name { get; set; }

    [Column("surname")]
    public string? Surname { get; set; }

    [Column("email")]
    public string Email { get; set; } = null!;

    [Column("password")]
    public string Password { get; set; } = null!;

    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("academic_role")]
    public string? AcademicRole { get; set; }

    [InverseProperty("AdminNavigation")]
    public virtual Admin? Admin { get; set; }

    [InverseProperty("StudentNavigation")]
    public virtual Student? Student { get; set; }

    [InverseProperty("TeacherNavigation")]
    public virtual Teacher? Teacher { get; set; }
}
