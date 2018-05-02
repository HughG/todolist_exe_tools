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
            var startDate = task.StartDate?.Date;
            if (startDate != null) {
                var passed = startDate.Value < DateTime.Today;
                var due = startDate.Value == DateTime.Today;
                writer.Write(" start {0}{1}{2}", passed ? "overdue " : "", due ? "due " : "", startDate.Value.ToShortDateString());
            }
            if (task.IsComplete) {
                writer.Write(" done");
            }
            if (task.IsExplicitlyComplete) {
                writer.Write(" on {0}", task.DoneDate?.Date.ToShortDateString());
            }
            if (task.HasOpenSubtasks) {
                writer.Write(" hasOpenSub");
            }
            if (task.HasCompletedAncestorTasks )
            {
                writer.Write(" hasCompletedAncestors");
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
