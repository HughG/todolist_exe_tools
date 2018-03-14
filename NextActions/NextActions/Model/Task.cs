using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextActions.Model
{
    class Task
    {
        public int Id { get; private set; }

        public Task(int id) {
            Id = id;
        }
    }
}
