using System.Collections.Generic;

namespace SudoEngine.Core
{
    public class GameObject : BaseObject
    {
        public static List<GameObject> AllGameObjects { get; set; } = new List<GameObject>();

        protected internal bool Started { get; private set; } = false;
        protected internal GameObject Parent { get; private set; } = null;
        protected internal List<GameObject> Childrens { get; private set; } = new List<GameObject>();

        public GameObject(string name = "GameObject") : base(name)
        {
            OnCreation();
            AllGameObjects.Add(this);
        }

        /// <summary>
        /// Supprime le GameObject ainsi que tous ces enfants
        /// </summary>
        public override void Delete()
        {
            OnDelete();
            if (Childrens.Count != 0) foreach (GameObject GO in Childrens) GO.Delete();
            AllGameObjects.Remove(this);
            base.Delete();
        }

        /// <summary>
        /// Active ou désactive l'objet ainsi que tous ces enfants
        /// </summary>
        /// <param name="status">Booléen indiquant le nouvel état de l'objet>/param>
        public override void SetEnable(bool status)
        {
            if (status)
            {
                OnEnable();
                if (Childrens.Count != 0) foreach (GameObject GO in Childrens.FindAll(GameObject => status)) GO.SetEnable(true);
            }
            else
            {
                OnDisable();
                if (Childrens.Count != 0) foreach (GameObject GO in Childrens.FindAll(GameObject => !status)) GO.SetEnable(false);
            }
            base.SetEnable(status);
        }

        public static void Update()
        {
            foreach (GameObject GO in AllGameObjects)
            {
                if (!GO.Enabled && !GO.Deleted) continue;
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
                if (!GO.Enabled && !GO.Deleted) continue;
                GO.OnRender();
            }
        }
        
        /// <summary>
        /// Permet d'assigner un objet en tant que parent de l'objet actuel
        /// </summary>
        /// <param name="parent">L'objet a mettre en parent</param>
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

        /// <summary>
        /// Écrit dans la console la hiérarchie de l'objet actuel (remonte l'arbre des parents)
        /// </summary>
        public void Hierarchy()
        {
            if (!Parent)
            {
                Log.Info("Aucune hiérarchie pour cet objet");
                return;
            }
            Log.Warning($"Hiérarchie de {Name}");
            GameObject gameObject = this;
            int a = 0;
            while (gameObject.Parent)
            {
                Log.Info($"Parent N°{a} : {gameObject.Parent.Name}");
                gameObject = gameObject.Parent;
                a++;
            }
        }

        public void Children(int index = 0)
        {
            if (Childrens.Count == 0)
            {
                Log.Info($"Cet objet ({Name}) n'a aucun enfant");
                return;
            }
            string indent = "";
            for (int x = 0; x < index; x++) indent += "   ";
            for (int i = 0; i < Childrens.Count; i++)
            {
                Log.Info($"{indent}Enfant N°{i} : {Childrens[i].Name}");
                if (Childrens[i].Childrens.Count != 0) Childrens[i].Children(index + 1);
            }
        }

        /// <summary>Invoqué lors de l'appel du constructeur</summary>
        protected internal virtual void OnCreation() { }
        /// <summary>Invoqué lors de la première frame active</summary>
        protected internal virtual void OnStart() { }
        /// <summary>Invoqué à chaque passage dans l'event OnUpdate de la fenêtre (La méthode statique <see cref="Update"/> doit y être appelé)</summary>
        protected internal virtual void OnUpdate() { }
        /// <summary>Invoqué à chaque passage dans l'event OnRender de la fenêtre (La méthode statique <see cref="Render"/> doit y être appelé)</summary>
        protected internal virtual void OnRender() { }
        /// <summary>Invoqué lorsque la méthode <see cref="Delete"/> est appelé</summary>
        protected internal virtual void OnDelete() { }
        /// <summary>Invoqué lorsque l'objet est activé avec la méthode <see cref="SetEnable(bool)"/></summary>
        protected internal virtual void OnEnable() { }
        /// <summary>Invoqué lorsque l'objet est déactivé avec la méthode <see cref="SetEnable(bool)"/></summary>
        protected internal virtual void OnDisable() { }
    }
}
