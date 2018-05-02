using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextActions.Model
{
    class Task : ITaskContainer
    {
        public uint Id { get; private set; }
        private WeakReference parent;
        public Task Parent { get { return (Task)parent.Target; } }
        //public uint ParentId { get; private set; } // 0 means it's a root task
        public ICollection<Task> Subtasks { get; private set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? DoneDate { get; private set; }
        public bool IsExplicitlyComplete { get { return DoneDate != null; } }
        public bool IsComplete { get {
                return IsExplicitlyComplete
                    || (Subtasks.Count > 0 && Subtasks.All(t => t.IsComplete));
            } }

        private Lazy<bool> hasOpenSubtasks;
        public bool HasOpenSubtasks { get { return hasOpenSubtasks.Value; } }

        private Lazy<bool> hasCompletedAncestorTasks;
        public bool HasCompletedAncestorTasks { get { return hasCompletedAncestorTasks.Value; } }

        public bool StartsInFuture { get { return StartDate?.Date > DateTime.Today; } }

        private Task(
            uint id,
            Task parentTask) {
            Id = id;
            parent = new WeakReference(parentTask);
            hasOpenSubtasks = new Lazy<bool>(() => Subtasks.Any(t => !t.IsExplicitlyComplete));
            hasCompletedAncestorTasks = new Lazy<bool>(() => {
                return Parent != null
                    && (Parent.IsExplicitlyComplete || Parent.HasCompletedAncestorTasks);
                });
        }

        public class Builder
        {
            private Task task;

            public Builder(
                uint id,
                Task parentTask,
                Func<Task, IEnumerable<Task>> buildSubtasks) {
                task = new Task(id, parentTask);
                task.Subtasks = new List<Task>(buildSubtasks(task));
            }

            public DateTime StartDate { set { task.StartDate = value; } }
            public DateTime DoneDate { set { task.DoneDate = value; } }

            public Task GetTask() { return task; }
        }
    }
}
