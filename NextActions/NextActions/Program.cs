using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NextActions.Formatters.Text;
using NextActions.Parsers.Xml;

namespace NextActions
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            if (args.Length > 0) {
                var toDoListFilename = args[0];
                Console.WriteLine(toDoListFilename);
                DumpToDoList(toDoListFilename);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void DumpToDoList(string toDoListFilename) {
            var parser = new ToDoListXmlParser();
            var list = parser.ParseFromFile(toDoListFilename);
            var formatter = new ToDoListTextFormatter(Console.Out);
            formatter.Format(list);
        }
    }
}
