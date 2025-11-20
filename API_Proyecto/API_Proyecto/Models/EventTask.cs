using System;
using System.Collections.Generic;

namespace API_Proyecto.Models;

public partial class EventTask
{
    public Guid EventId { get; set; }

    public Guid TaskId { get; set; }

    public int? TaskOrder { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;
}
