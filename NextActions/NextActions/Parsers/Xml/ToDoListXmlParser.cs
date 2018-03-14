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
        public ToDoList ParseFromFile(string filename) {
            XDocument doc = XDocument.Load(File.OpenRead(filename));

            return new ToDoList(
                projectName: (string)doc.Root.Attribute("PROJECTNAME"),
                tasks: from t in doc.Descendants("TASK")
                       select new Task((int)t.Attribute("ID"))
                );
        }
    }
}
