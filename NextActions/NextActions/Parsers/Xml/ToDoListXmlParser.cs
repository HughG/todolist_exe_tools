using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml.Linq;
using NextActions.Model;

namespace NextActions.Parsers.Xml
{
    class ToDoListXmlParser
    {
        private Task ParseTask(IDictionary<uint, Task> allTasks, uint parentTaskId, XElement taskElement)
        {
            var builder = new Task.Builder(
                allTasks,
                GetTaskId(taskElement),
                parentTaskId,
                p => ParseChildTasks(allTasks, p.Id, taskElement));

            var startDateString = (string)taskElement.Attribute("STARTDATESTRING");
            if (startDateString != null) {
                builder.StartDate = DateTime.Parse(startDateString);
            }

            var doneDateString = (string)taskElement.Attribute("DONEDATESTRING");
            if (doneDateString != null) {
                builder.DoneDate = DateTime.Parse(doneDateString);
            }

            var priorityString = (string)taskElement.Attribute("PRIORITY");
            if (priorityString != null) {
                var priority = SByte.Parse(priorityString);
                if (priority >= 0) {
                    builder.Priority = (byte)priority;
                }
            }

            var dependencyIds =
                from d in taskElement.Elements("DEPENDS")
                select UInt32.Parse(d.Value);
            builder.SetDependencyIds(dependencyIds);

        Task task = builder.GetTask();
            allTasks[task.Id] = task;
            return task;
        }

        private IEnumerable<Task> ParseChildTasks(IDictionary<uint, Task> allTasks, uint parentTaskId, XElement parentElement) {
            return from t in parentElement.Elements("TASK")
                   select ParseTask(allTasks, parentTaskId, t);
        }

        private uint GetTaskId(XElement element) {
            var elementName = element.Name.ToString();
            switch (elementName) {
                case "TODOLIST":
                    return 0;
                case "TASK":
                    return (uint)element.Attribute("ID");
                default:
                    throw new IOException("Element of type {0} is not a valid task");
            }
        }

        public ToDoList ParseFromFile(string filename) {
            XDocument doc = XDocument.Load(File.OpenRead(filename));
            XElement root = doc.Root;
            var allTasks = new Dictionary<uint, Task>();

            return new ToDoList(
                projectName: (string)root.Attribute("PROJECTNAME"),
                rootTasks: ParseChildTasks(allTasks, 0, root),
                allTasks: allTasks
                );
        }
    }
}
