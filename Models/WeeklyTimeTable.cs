using System;
using System.Collections.Generic;

namespace PRN211_Project.Models;

public partial class WeeklyTimeTable
{
    public int Id { get; set; }

    public int? RoomsId { get; set; }

    public int? TeachersId { get; set; }

    public int? CourseId { get; set; }

    public int? ClassId { get; set; }

    public int? TimeSlotId { get; set; }

    public DateTime? LearnDate { get; set; }

    public string? Description { get; set; }

    public virtual ClassRoom? Class { get; set; }

    public virtual Course? Course { get; set; }

    public virtual Room? Rooms { get; set; }

    public virtual Teacher? Teachers { get; set; }

    public virtual TimeSlot? TimeSlot { get; set; }
}
