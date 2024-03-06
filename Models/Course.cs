using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PRN211_Project.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string? CourseName { get; set; }

    public string? CourseCode { get; set; }
    [JsonIgnore]
    public virtual ICollection<TeacherDetail> TeacherDetails { get; } = new List<TeacherDetail>();
    [JsonIgnore]
    public virtual ICollection<WeeklyTimeTable> WeeklyTimeTables { get; } = new List<WeeklyTimeTable>();
}
