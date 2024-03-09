using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PRN211_Project.Models;

public partial class ClassRoom
{
    public int ClassId { get; set; }

    public string? ClassName { get; set; }

    public string? Description { get; set; }

    [JsonIgnore]
    public virtual ICollection<TeacherDetail> TeacherDetails { get; } = new List<TeacherDetail>();
    [JsonIgnore]
    public virtual ICollection<WeeklyTimeTable> WeeklyTimeTables { get; } = new List<WeeklyTimeTable>();
    [JsonIgnore]
    public virtual ICollection<TeacherClass> TeacherClasses { get; } = new List<TeacherClass>();

}
