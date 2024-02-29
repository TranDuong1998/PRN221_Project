using System;
using System.Collections.Generic;

namespace PRN221_Project.Models;

public partial class TeacherDetail
{
    public int Id { get; set; }

    public int? TeacherId { get; set; }

    public int? CourseId { get; set; }

    public virtual Course? Course { get; set; }

    public virtual Teacher? Teacher { get; set; }
}
