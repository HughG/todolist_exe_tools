using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextActions.Util
{
    class Lazy<T>
    {
        private T value = default(T);
        private Func<T> f;

        public Lazy(Func<T> f) {
            if (f == null) {
                throw new ArgumentNullException("f");
            }
            this.f = f;
        }

        implict T() {
            if (f != null) {
                value = f();
                f = null;
            }
            return value;
        }
    }
}
