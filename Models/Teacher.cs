using System;
using System.Collections.Generic;

namespace PRN221_Project.Models;

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

    public virtual ICollection<TeacherDetail> TeacherDetails { get; } = new List<TeacherDetail>();

    public virtual ICollection<WeeklyTimeTable> WeeklyTimeTables { get; } = new List<WeeklyTimeTable>();
}
