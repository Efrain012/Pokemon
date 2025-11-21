using System;
using System.Collections.Generic;

namespace API_Proyecto.Models;

public partial class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Type { get; set; } = null!;
    public Guid? CreatorId { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
}