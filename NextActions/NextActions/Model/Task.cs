using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextActions.Model
{
    class Task : ITaskContainer
    {
        public uint Id { get; private set; }
        public uint ParentId { get; private set; } // 0 means it's a root task
        public ICollection<Task> Subtasks { get; private set; }
        public DateTime? DoneDate { get; private set; }
        public bool IsComplete { get { return DoneDate != null; } }

        private Lazy<bool> hasOpenSubtasks = new Lazy(() => Subtasks.Any(t => !t.IsComplete));
        public bool HasOpenSubtasks { get { (bool)hasOpenSubtasks;
            }
        }

        private Task(
            uint id,
            uint parentId,
            IEnumerable<Task> subtasks) {
            Id = id;
            ParentId = ParentId;
            Subtasks = new List<Task>(subtasks);
        }

        public class Builder
        {
            private Task task;

            public Builder(
                uint id,
                uint parentId,
                IEnumerable<Task> subtasks) {
                task = new Task(id, parentId, subtasks);
            }

            public DateTime DoneDate { set { task.DoneDate = value; } }

            public Task GetTask() { return task; }
        }
    }
}
