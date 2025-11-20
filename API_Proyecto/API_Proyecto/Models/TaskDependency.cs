using System;
using System.Collections.Generic;

namespace API_Proyecto.Models;

public partial class TaskDependency
{
    public Guid PrerequisiteTaskId { get; set; }

    public Guid DependentTaskId { get; set; }
}
