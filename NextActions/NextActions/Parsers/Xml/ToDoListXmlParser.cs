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
        private IEnumerable<Task> ParseChildTasks(XElement parent) {
            var parentTaskId = GetTaskId(parent);
            return from t in parent.Elements("TASK")
                   select new Task(
                       GetTaskId(t),
                       parentTaskId,
                       ParseChildTasks(t));
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
                rootTasks: ParseChildTasks(root)
                );
        }
    }
}
