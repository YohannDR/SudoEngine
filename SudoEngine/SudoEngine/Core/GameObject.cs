using System.Collections.Generic;

namespace SudoEngine.Core
{
    public abstract class GameObject : BaseObject
    {
        public static List<GameObject> AllGameObjects { get; set; } = new List<GameObject>();
        protected internal bool Started { get; private set; } = false;

        public GameObject(string name = "BaseObjet") : base(name)
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

        public override void SetEnable(bool status)
        {
            if (status) OnEnable();
            else OnDisable();
            base.SetEnable(status);
        }

        public static void Update()
        {
            foreach (GameObject GO in AllGameObjects)
            {
                if (GO.Enabled && !GO.Deleted) continue;
                if (!GO.Started)
                {
                    GO.OnStart();
                    GO.Started = true;
                }
                GO.OnUpdate();
            }
        }

        public static void Render()
        {
            foreach (GameObject GO in AllGameObjects)
            {
                if (GO.Enabled && !GO.Deleted) continue;
                GO.OnRender();
            }
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
