using System.Collections.Generic;

namespace SudoEngine.Core
{
    public abstract class GameObject : BaseObject
    {
        public static List<GameObject> AllGameObjects { get; set; } = new List<GameObject>();

        //public Module Module = new Module();

        public GameObject() : base()
        {
            OnCreation();
            AllGameObjects.Add(this);
        }

        public GameObject(string name) : base(name)
        {
            OnCreation();
            AllGameObjects.Add(this);
        }

        public override void Delete()
        {
            OnDelete();
            AllGameObjects.Remove(this);
            base.Delete();
        }

        protected internal override void SetEnable(bool status)
        {
            if (status) OnEnable();
            else OnDisable();
            base.SetEnable(status);
        }

        

        protected internal virtual void OnCreation() { }
        protected internal virtual void OnStart() { }
        protected internal virtual void OnUpdate() { }
        protected internal virtual void OnRender() { }
        protected internal virtual void OnDelete() { }

        protected internal virtual void OnEnable() { }
        protected internal virtual void OnDisable() { }
    }
}
