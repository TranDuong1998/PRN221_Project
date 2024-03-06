using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PRN211_Project.Models;

public partial class Teacher
{
    public int TeacherId { get; set; }

    public int? AccountId { get; set; }

    public string? TeachersCode { get; set; }

    public string? FullName { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public DateTime? Dob { get; set; }

    public virtual Account? Account { get; set; }
    [JsonIgnore]
    public virtual ICollection<TeacherDetail> TeacherDetails { get; } = new List<TeacherDetail>();
    [JsonIgnore]
    public virtual ICollection<WeeklyTimeTable> WeeklyTimeTables { get; } = new List<WeeklyTimeTable>();
}
