using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextActions.Model
{
    interface ITaskContainer
    {
        ICollection<Task> Subtasks { get; }
    }
}
