using System;
using System.Collections.Generic;

namespace PRN221_Project.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string? CourseName { get; set; }

    public string? CourseCode { get; set; }

    public virtual ICollection<TeacherDetail> TeacherDetails { get; } = new List<TeacherDetail>();

    public virtual ICollection<WeeklyTimeTable> WeeklyTimeTables { get; } = new List<WeeklyTimeTable>();
}
