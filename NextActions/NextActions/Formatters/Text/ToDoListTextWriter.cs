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

        private void FormatChildTasks(ITaskContainer taskContainer, string indent = "") {
            foreach (var task in taskContainer.ChildTasks) {
                writer.WriteLine("{0}{1}", indent, task.Id);
                FormatChildTasks(task, indent + "  ");
            }
        }

        public void Format(ToDoList toDoList) {
            writer.WriteLine("To-Do List '{0}'", toDoList.ProjectName);
            writer.WriteLine("Tasks");
            FormatChildTasks(toDoList);
        }
    }
}
