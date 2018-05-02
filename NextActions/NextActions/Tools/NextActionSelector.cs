using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NextActions.Model;

namespace NextActions.Tools
{
    class NextActionSelector
    {
        public NextActionSelector() { }

        public IEnumerable<Task> SelectTasks(ToDoList list) {
            return list.AllTasks.Values
                .Where(t => IsNextAction(t));
        }

        private bool IsNextAction(Task t) {
            return !t.IsComplete
                && !t.HasOpenSubtasks
                && !t.HasCompletedAncestorTasks
                && !t.StartsInFuture
                && !t.HasOpenDependencies;
        }
    }
}
