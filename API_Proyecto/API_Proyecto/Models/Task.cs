using System;
using System.Collections.Generic;

namespace API_Proyecto.Models;

public partial class Task
{
    public Guid Id { get; set; }

    public Guid? ProjectId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Priority { get; set; } = null!;

    public string Type { get; set; } = null!;

    public bool Active { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<EventTask> EventTasks { get; set; } = new List<EventTask>();

    public virtual Project? Project { get; set; }

    public virtual ICollection<Task> DependentTasks { get; set; } = new List<Task>();

    public virtual ICollection<Task> PrerequisiteTasks { get; set; } = new List<Task>();
}
