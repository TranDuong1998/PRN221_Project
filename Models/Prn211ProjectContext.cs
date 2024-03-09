using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PRN211_Project.Models;

namespace PRN211_Project.Models;

public partial class Prn211ProjectContext : DbContext
{
    public Prn211ProjectContext()
    {
    }

    public Prn211ProjectContext(DbContextOptions<Prn211ProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<ClassRoom> ClassRooms { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<TeacherClass> TeacherClasses { get; set; }

    public virtual DbSet<TeacherDetail> TeacherDetails { get; set; }

    public virtual DbSet<TimeSlot> TimeSlots { get; set; }

    public virtual DbSet<WeeklyTimeTable> WeeklyTimeTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        IConfigurationRoot configuration = builder.Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyDb"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("pk_Account");

            entity.HasIndex(e => e.Email, "UQ__Accounts__A9D10534BADC8DA7").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__Accounts__C9F28456AAEA65F7").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ClassRoom>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("pk_ClassRooms");

            entity.Property(e => e.ClassName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description).HasMaxLength(255);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("pk_Course");

            entity.HasIndex(e => e.CourseCode, "UQ__Courses__FC00E000DD885611").IsUnique();

            entity.Property(e => e.CourseCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CourseName).HasMaxLength(255);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomsId).HasName("pk_Rooms");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.RoomsName).HasMaxLength(50);
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("pk_Teachers");

            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("DOB");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TeachersCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("fk_TeacherAccount");
        });

        modelBuilder.Entity<TeacherClass>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_TeacherClasses");

            entity.HasOne(d => d.Class).WithMany(p => p.TeacherClasses)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("fk_TeacherClassesClassRooms");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TeacherClasses)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("fk_TeacherClassesTeacher");
        });

        modelBuilder.Entity<TeacherDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_TeacherDetails");

            entity.HasOne(d => d.Course).WithMany(p => p.TeacherDetails)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("fk_TeacherDetailCourse");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TeacherDetails)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("fk_TeacherDetailTeacher");
        });

        modelBuilder.Entity<TimeSlot>(entity =>
        {
            entity.HasKey(e => e.TimeSlotId).HasName("pk_TimeSlot");

            entity.ToTable("TimeSlot");

            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<WeeklyTimeTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_WeeklyTimeTable");

            entity.ToTable("WeeklyTimeTable");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.LearnDate).HasColumnType("date");

            entity.HasOne(d => d.Class).WithMany(p => p.WeeklyTimeTables)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("fk_WeeklyTimeTableClassRooms");

            entity.HasOne(d => d.Course).WithMany(p => p.WeeklyTimeTables)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("fk_WeeklyTimeTableCourse");

            entity.HasOne(d => d.Rooms).WithMany(p => p.WeeklyTimeTables)
                .HasForeignKey(d => d.RoomsId)
                .HasConstraintName("fk_WeeklyTimeTableRooms");

            entity.HasOne(d => d.Teachers).WithMany(p => p.WeeklyTimeTables)
                .HasForeignKey(d => d.TeachersId)
                .HasConstraintName("fk_WeeklyTimeTableTeachers");

            entity.HasOne(d => d.TimeSlot).WithMany(p => p.WeeklyTimeTables)
                .HasForeignKey(d => d.TimeSlotId)
                .HasConstraintName("fk_WeeklyTimeTableTime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
