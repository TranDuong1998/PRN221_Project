using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PRN211_Project.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }

    public bool? Active { get; set; }
    [JsonIgnore]
    public virtual ICollection<Teacher> Teachers { get; } = new List<Teacher>();
}
