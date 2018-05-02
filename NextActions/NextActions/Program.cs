using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NextActions.Formatters.Text;
using NextActions.Model;
using NextActions.Parsers.Xml;
using NextActions.Tools;

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

                var parser = new ToDoListXmlParser();
                var list = parser.ParseFromFile(toDoListFilename);

                Console.WriteLine();
                DumpToDoList(list);

                Console.WriteLine();
                DumpNextActions(list);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void DumpToDoList(ToDoList list) {
            new ToDoListTextFormatter(Console.Out).Format(list);
        }
        private static void DumpNextActions(ToDoList list) {
            var nextActions = new NextActionSelector().SelectTasks(list);
            foreach(var t in nextActions) {
                Console.WriteLine("{0}", t.Id);
            }

        }
    }
}
