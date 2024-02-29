using System;
using System.Collections.Generic;

namespace PRN221_Project.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<Teacher> Teachers { get; } = new List<Teacher>();
}
