using System;
using System.Collections.Generic;
using System.Linq;

namespace NextActions.Model
{
    class Task : ITaskContainer
    {
        private IDictionary<uint, Task> allTasks;
        private uint parentId; // 0 means it's a root task

        public uint Id { get; private set; }
        public Task Parent { get {
                allTasks.TryGetValue(parentId, out Task result);
                return result;
            } }
        public ICollection<Task> Subtasks { get; private set; }
        private ISet<uint> dependencyIds = new HashSet<uint>();
        public DateTime? StartDate { get; private set; }
        public DateTime? DoneDate { get; private set; }
        public byte? Priority { get; private set; }
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

        private Lazy<IDictionary<uint, Task>> dependencyTasks;
        public IDictionary<uint, Task> DependencyTasks { get { return dependencyTasks.Value; } }

        private Lazy<bool> hasOpenDependencies;
        public bool HasOpenDependencies { get { return hasOpenDependencies.Value; } }

        #region Builder support

        private Task(
            IDictionary<uint, Task> allTasks,
            uint id,
            uint parentId
            ) {
            this.allTasks = allTasks;
            this.parentId = parentId;
            Id = id;
            hasOpenSubtasks = new Lazy<bool>(() => Subtasks.Any(t => !t.IsExplicitlyComplete));
            hasCompletedAncestorTasks = new Lazy<bool>(() =>
            {
                return Parent != null
                    && (Parent.IsExplicitlyComplete || Parent.HasCompletedAncestorTasks);
            });
            dependencyTasks = new Lazy<IDictionary<uint, Task>>(() =>
            {
                return allTasks
                    .Where(kvp => dependencyIds.Contains(kvp.Key))
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            });
            hasOpenDependencies = new Lazy<bool>(() =>
            {
                return DependencyTasks.Values.Any(t => !t.IsComplete);
            });
        }

        private void SetDependencyIds(IEnumerable<uint> ids) {
            dependencyIds = new HashSet<uint>(ids);
        }

        #endregion

        public class Builder
        {
            private Task task;

            public Builder(
                IDictionary<uint, Task> allTasks,
                uint id,
                uint parentId,
                Func<Task, IEnumerable<Task>> buildSubtasks
                ) {
                task = new Task(allTasks, id, parentId);
                task.Subtasks = new List<Task>(buildSubtasks(task));
            }

            public DateTime StartDate { set { task.StartDate = value; } }
            public DateTime DoneDate { set { task.DoneDate = value; } }
            public byte Priority { set { task.Priority = value; } }
            public void SetDependencyIds(IEnumerable<uint> ids) { task.SetDependencyIds(ids); }

            public Task GetTask() { return task; }
        }
    }
}
