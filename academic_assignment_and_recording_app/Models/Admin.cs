using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace academic_assignment_and_recording_app.Models;

[Table("admin")]
public partial class Admin
{
    [Key]
    [Column("admin_id")]
    public int AdminId { get; set; }

    [ForeignKey("AdminId")]
    [InverseProperty("Admin")]
    public virtual User AdminNavigation { get; set; } = null!;
}
