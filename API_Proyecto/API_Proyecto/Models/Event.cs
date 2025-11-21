using System;
using System.Collections.Generic;

namespace API_Proyecto.Models;

public partial class Event
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Type { get; set; } = null!;
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
}