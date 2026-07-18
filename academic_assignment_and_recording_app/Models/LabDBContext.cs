using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace academic_assignment_and_recording_app.Models;

public partial class LabDBContext : DbContext
{
    public LabDBContext()
    {
    }

    public LabDBContext(DbContextOptions<LabDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=db;Port=5432;Database=temporary_29;Username=postgres;Password=12345;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("admin_pkey");

            entity.Property(e => e.AdminId).ValueGeneratedNever();

            entity.HasOne(d => d.AdminNavigation).WithOne(p => p.Admin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("admin_admin_id_fkey");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("course_pkey");

            entity.Property(e => e.CourseId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("student_pkey");

            entity.Property(e => e.StudentId).ValueGeneratedNever();

            entity.HasOne(d => d.StudentNavigation).WithOne(p => p.Student)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("student_student_id_fkey");

            entity.HasMany(d => d.Courses).WithMany(p => p.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentCourse",
                    r => r.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("student_course_course_id_fkey"),
                    l => l.HasOne<Student>().WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("student_course_student_id_fkey"),
                    j =>
                    {
                        j.HasKey("StudentId", "CourseId").HasName("student_course_pkey");
                        j.ToTable("student_course");
                        j.IndexerProperty<int>("StudentId").HasColumnName("student_id");
                        j.IndexerProperty<int>("CourseId").HasColumnName("course_id");
                    });
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("subject_pkey");

            entity.HasOne(d => d.Course).WithMany(p => p.Subjects).HasConstraintName("subject_course_id_fkey");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("task_pkey");

            entity.Property(e => e.TaskId).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Student).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("task_student_id_fkey");

            entity.HasOne(d => d.Subject).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("task_subject_id_fkey");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("teacher_pkey");

            entity.Property(e => e.TeacherId).ValueGeneratedNever();

            entity.HasOne(d => d.TeacherNavigation).WithOne(p => p.Teacher)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("teacher_teacher_id_fkey");

            entity.HasMany(d => d.Courses).WithMany(p => p.Teachers)
                .UsingEntity<Dictionary<string, object>>(
                    "TeacherCourse",
                    r => r.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("teacher_course_course_id_fkey"),
                    l => l.HasOne<Teacher>().WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("teacher_course_teacher_id_fkey"),
                    j =>
                    {
                        j.HasKey("TeacherId", "CourseId").HasName("teacher_course_pkey");
                        j.ToTable("teacher_course");
                        j.IndexerProperty<int>("TeacherId").HasColumnName("teacher_id");
                        j.IndexerProperty<int>("CourseId").HasColumnName("course_id");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_pkey");

            entity.Property(e => e.UserId).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
