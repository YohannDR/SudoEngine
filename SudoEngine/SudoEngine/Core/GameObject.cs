using OpenTK.Input;
using System.Collections.Generic;

namespace SudoEngine.Core
{
    /// <summary>
    /// Classe <see langword="abstract"/> qui fournit du scripting et un système de hiérarchie d'objet
    /// <para>Hérite de <see cref="BaseObject"/> et doit être hérité pour être utilisé</para>
    /// </summary>
    public abstract class GameObject : BaseObject
    {
        /// <summary>Liste de tous les <see cref="GameObject"/> chargés en mémoire</summary>
        public static List<GameObject> AllGameObjects { get; set; } = new List<GameObject>();

        /// <summary><see cref="bool"/> indiquant si le GameObject est passé par l'évenement <see cref="OnStart"/></summary>
        protected internal bool Started { get; private set; } = false;

        /// <summary>Le <see cref="GameObject"/> assigné en parent de ce GameObject, <see langword="null"/> si aucun parent</summary>
        protected internal GameObject Parent { get; private set; } = null;

        /// <summary>Liste des enfants de ce GameObject</summary>
        protected internal List<GameObject> Childrens { get; private set; } = new List<GameObject>();

        /// <summary>
        /// Crée un nouvel objet <see cref="GameObject"/> et appele le constructeur de <see cref="BaseObject"/>
        /// </summary>
        /// <param name="name">Le nom interne de l'objet (GameObject par défaut)</param>
        protected internal GameObject(string name = "GameObject") : base(name)
        {
            OnCreation();
            AllGameObjects.Add(this);
        }

        /// <summary>Supprime le GameObject ainsi que tous ces enfants</summary>
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
        /// <param name="status">Booléen indiquant le nouvel état de l'objet</param>
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

        /// <summary>Update tous les <see cref="GameObject"/></summary>
        public static void Update()
        {
            foreach (GameObject GO in AllGameObjects)
            {
                if (GO.Enabled)
                {
                    if (!GO.Started)
                    {
                        GO.OnStart();
                        GO.Started = true;
                    }
                    GO.OnUpdate();
                }
            }
        }

        /// <summary>Render tous les <see cref="GameObject"/></summary>
        public static void Render()
        {
            foreach (GameObject GO in AllGameObjects)
            {
                if (GO.Enabled) GO.OnRender();
            }
        }

        public static void KeyDown(KeyboardKeyEventArgs e)
        {
            foreach (GameObject GO in AllGameObjects)
            {
                if (GO.Enabled) GO.OnKeyDown(e);
            }
        }

        public static void KeyUp(KeyboardKeyEventArgs e)
        {
            foreach (GameObject GO in AllGameObjects)
            {
                if (GO.Enabled) GO.OnKeyUp(e);
            }
        }

        /// <summary>
        /// Permet d'assigner un objet en tant que parent du GameObject actuel
        /// </summary>
        /// <param name="parent">L'objet a mettre en parent</param>
        public void SetParent(GameObject parent)
        {
            if (parent.Deleted)
            {
                Log.Error("Impossible d'assigner un GameObject supprimé en tant que parent");
                return;
            }

            if (parent == this)
            {
                Log.Error("Impossible d'assigner un GameObject en tant que parent de lui même");
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

        /// <summary>Écrit dans la console la hiérarchie du GameObject actuel (remonte l'arbre des parents)</summary>
        public void Hierarchy()
        {
            if (!Parent)
            {
                Log.Info("Aucune hiérarchie pour cet objet");
                return;
            }
            Log.Warning($"Hiérarchie de {Name}");
            GameObject gameObject = this;
            for (int a = 0; gameObject.Parent; a++)
            {
                Log.Info($"Parent N°{a} : {gameObject.Parent.Name}");
                gameObject = gameObject.Parent;
            }
        }

        /// <summary>
        /// Écrit dans la console la liste des enfants du GameObject actuel (écrit également les enfants des enfants)
        /// </summary>
        /// <param name="index">Paramètre utilisé pour la rrécursivité, ne doit pas être modifié</param>
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

        /// <summary>Invoqué lorsque l'objet est désactivé avec la méthode <see cref="SetEnable(bool)"/></summary>
        protected internal virtual void OnDisable() { }

        /// <summary>Invoqué à chaque passage dans l'event OnKeyDown de la fenêtre (La méthode statique <see cref="KeyDown"/> doit y être appelé)</summary>
        protected internal virtual void OnKeyDown(KeyboardKeyEventArgs e) { }

        /// <summary>Invoqué à chaque passage dans l'event OnKeyUp de la fenêtre (La méthode statique <see cref="KeyUp"/> doit y être appelé)</summary>
        protected internal virtual void OnKeyUp(KeyboardKeyEventArgs e) { }
    }
}