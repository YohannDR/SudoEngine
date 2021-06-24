using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudoEngine.Core
{
    public abstract class Module : BaseObject
    {
        public Module() : base() { }

        protected internal virtual void OnLoad() { }
        protected internal virtual void OnStart() { }
        protected internal virtual void OnUpdate() { }
        protected internal virtual void OnRender() { }
        protected internal virtual void OnUnload() { }

        protected internal virtual void OnEnable() { }
        protected internal virtual void OnDisable() { }
    }
}
