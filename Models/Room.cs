using System;
using System.Collections.Generic;

namespace PRN221_Project.Models;

public partial class Room
{
    public int RoomsId { get; set; }

    public string? RoomsName { get; set; }

    public int? TimeSlotId { get; set; }

    public virtual TimeSlot? TimeSlot { get; set; }

    public virtual ICollection<WeeklyTimeTable> WeeklyTimeTables { get; } = new List<WeeklyTimeTable>();
}
