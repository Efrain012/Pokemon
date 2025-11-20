using System;
using System.Collections.Generic;

namespace API_Proyecto.Models;

public partial class ProjectMember
{
    public Guid ProjectId { get; set; }

    public Guid PersonId { get; set; }

    public string Role { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}
