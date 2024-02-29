using System;
using System.Collections.Generic;

namespace PRN221_Project.Models;

public partial class TimeSlot
{
    public int TimeSlotId { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Room> Rooms { get; } = new List<Room>();
}
