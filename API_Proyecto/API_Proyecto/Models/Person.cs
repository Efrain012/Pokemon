using System;
using System.Collections.Generic;

namespace API_Proyecto.Models;

public partial class Person
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public bool Active { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
}
