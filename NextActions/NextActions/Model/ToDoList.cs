using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextActions.Model
{
    class ToDoList
    {
        public string ProjectName { get; private set; }
        public IDictionary<int, Task> Tasks { get; private set; }

        public ToDoList(string projectName, IEnumerable<Task> tasks) {
            ProjectName = projectName;
            Tasks = new Dictionary<int, Task>();
            foreach (var task in tasks) {
                var id = task.Id;
                if (Tasks.ContainsKey(id)) {
                    Console.Error.WriteLine("WARNING: Task ID {0} appears more than once.", id);
                }
                Tasks[id] = task;
            }
        }
    }
}
