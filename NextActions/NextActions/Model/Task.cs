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
        public ICollection<Task> ChildTasks { get; private set; }

        public Task(uint id, uint parentId, IEnumerable<Task> childTasks) {
            Id = id;
            ParentId = ParentId;
            ChildTasks = new List<Task>(childTasks);
        }
    }
}
