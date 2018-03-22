using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextActions.Model
{
    class ToDoList : ITaskContainer
    {
        public string ProjectName { get; private set; }
        public ICollection<Task> Subtasks { get; private set; }
        public 

        public ToDoList(string projectName, IEnumerable<Task> rootTasks) {
            ProjectName = projectName;
            Subtasks = new List<Task>(rootTasks);
        }
    }
}
