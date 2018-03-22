using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using NextActions.Model;

namespace NextActions.Formatters.Text
{
    class ToDoListTextFormatter
    {
        private TextWriter writer;

        public ToDoListTextFormatter(TextWriter writer) {
            this.writer = writer;
        }

        private void FormatTask(string indent, Task task) {
            writer.Write("{0}{1}", indent, task.Id);
            if (task.IsComplete) {
                writer.Write(" done {0}", task.DoneDate.Value.Date);
            }
            if (task.HasOpenSubtasks) {
                writer.Write(" hasOpenSub");
            }
            writer.WriteLine();
            FormatChildTasks(task, indent + "  ");
        }

        private void FormatChildTasks(ITaskContainer taskContainer, string indent = "") {
            foreach (var task in taskContainer.Subtasks) {
                FormatTask(indent, task);
            }
        }

        public void Format(ToDoList toDoList) {
            writer.WriteLine("To-Do List '{0}'", toDoList.ProjectName);
            writer.WriteLine("Tasks");
            FormatChildTasks(toDoList);
        }
    }
}
