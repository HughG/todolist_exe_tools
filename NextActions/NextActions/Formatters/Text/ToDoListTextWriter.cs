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

        public void Format(ToDoList toDoList) {
            writer.WriteLine("To-Do List '{0}'", toDoList.ProjectName);
            writer.WriteLine("Tasks");
            foreach (var task in toDoList.Tasks.Values) {
                writer.WriteLine("  {0}", task.Id);
            }
        }
    }
}
