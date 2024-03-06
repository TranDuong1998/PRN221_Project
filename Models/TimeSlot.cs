using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PRN211_Project.Models;

public partial class TimeSlot
{
    public int TimeSlotId { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string? Description { get; set; }
    [JsonIgnore]
    public virtual ICollection<WeeklyTimeTable> WeeklyTimeTables { get; } = new List<WeeklyTimeTable>();
}
