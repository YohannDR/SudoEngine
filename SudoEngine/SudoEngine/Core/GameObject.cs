using System.Collections.Generic;

namespace SudoEngine.Core
{
    public class GameObject : BaseObject
    {
        public static List<GameObject> AllGameObjects { get; set; } = new List<GameObject>();

        public GameObject() : base() => AllGameObjects.Add(this);

        public GameObject(string name) : base(name) => AllGameObjects.Add(this);

        public override void Delete()
        {
            AllGameObjects.Remove(this);
            base.Delete();
        }
    }
}
