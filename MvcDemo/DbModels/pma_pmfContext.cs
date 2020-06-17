using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MvcDemo.AuthenticationMiddleware.CustomIdentityStores.BaseClasses;
using MvcDemo.AuthenticationMiddleware.CustomIdentityStores.StorageProviders;

namespace MvcDemo.DbModels
{
    public partial class pma_pmfContext : DbContext
    {
        public pma_pmfContext()
        {
        }

        public pma_pmfContext(DbContextOptions<pma_pmfContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appuser> Appuser { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<StudentCourse> StudentCourse { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=pma_pmf");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appuser>(entity =>
            {
                entity.ToTable("appuser");

                entity.HasIndex(e => e.Email)
                    .HasName("appuser_email_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.Id)
                    .HasName("appuser_id_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.Username)
                    .HasName("appuser_username_uindex")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasColumnName("firstname");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasColumnName("lastname");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnName("password_hash");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("course");

                entity.HasIndex(e => e.Id)
                    .HasName("course_id_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.Isvu)
                    .HasName("course_isvu_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DepartmentId).HasColumnName("department_id");

                entity.Property(e => e.Ects).HasColumnName("ects");

                entity.Property(e => e.Isvu)
                    .IsRequired()
                    .HasColumnName("isvu");

                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasColumnName("level");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.Shortname)
                    .IsRequired()
                    .HasColumnName("shortname");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Course)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("course_department_id_fk");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("department");

                entity.HasIndex(e => e.Id)
                    .HasName("department_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.Shortname)
                    .IsRequired()
                    .HasColumnName("shortname");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.HasIndex(e => e.Id)
                    .HasName("role_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("student");

                entity.HasIndex(e => e.Id)
                    .HasName("student_id_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.Jmbag)
                    .HasName("student_jmbag_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.UserId)
                    .HasName("student_user_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Jmbag)
                    .IsRequired()
                    .HasColumnName("jmbag");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.CourseId })
                    .HasName("student_course_pk");

                entity.ToTable("student_course");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.StudentCourse)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("student_course_course_id_fk");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentCourse)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("student_course_student_id_fk");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("user_role_pk");

                entity.ToTable("user_role");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_role_role_id_fk");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRole)
                    .HasPrincipalKey(p => p.Id)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_role_appuser_id_fk");
            });

            base.OnModelCreating(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
