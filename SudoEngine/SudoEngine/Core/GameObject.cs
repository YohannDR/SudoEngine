using System.Collections.Generic;

namespace SudoEngine.Core
{
    public class GameObject : BaseObject
    {
        public static List<GameObject> AllGameObjects { get; set; } = new List<GameObject>();

        protected internal bool Started { get; private set; } = false;
        protected internal GameObject Parent { get; private set; } = null;
        protected internal List<GameObject> Childrens { get; private set; } = new List<GameObject>();

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
        

        public void SetParent(GameObject parent)
        {
            if (parent.Deleted)
            {
                Log.Error("Impossible d'assigner un GameObject supprimé en tant que parent");
                return;
            }

            if (!parent)
            {
                Parent = null;
                return;
            }

            Parent = parent;
            parent.Childrens.Add(this);
            if (Enabled != Parent.Enabled) SetEnable(Parent.Enabled);
        }

        public void Hierarchy()
        {
            if (!Parent)
            {
                Log.Info("Aucune hiérarchie pour cet objet");
                return;
            }
            Log.Warning($"Hiérarchie de {Name}\n");
            GameObject gameObject = this;
            int a = 0;
            while (gameObject.Parent)
            {
                Log.Info($"Parent N°{a} : {gameObject.Parent.Name}");
                gameObject = gameObject.Parent;
                a++;
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
