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
        private Task ParseTask(Task parentTask, XElement taskElement)
        {
            var builder = new Task.Builder(
                GetTaskId(taskElement),
                parentTask,
                p => ParseChildTasks(p, taskElement));

            var startDateString = (string)taskElement.Attribute("STARTDATESTRING");
            if (startDateString != null) {
                builder.StartDate = DateTime.Parse(startDateString);
            }

            var doneDateString = (string)taskElement.Attribute("DONEDATESTRING");
            if (doneDateString != null) {
                builder.DoneDate = DateTime.Parse(doneDateString);
            } 
            return builder.GetTask();
        }

        private IEnumerable<Task> ParseChildTasks(Task parentTask, XElement parentElement) {
            return from t in parentElement.Elements("TASK")
                   select ParseTask(parentTask, t);
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

            return new ToDoList(
                projectName: (string)root.Attribute("PROJECTNAME"),
                rootTasks: ParseChildTasks(null, root)
                );
        }
    }
}
