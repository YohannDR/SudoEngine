using SudoEngine.Core;

namespace SudoEngine.Core
{
    public class Test : GameObject
    {
        protected internal override void OnCreation()
        {
            Log.Info($"Creation de {this}");
            base.OnCreation();
        }

        protected internal override void OnDisable()
        {
            Log.Warning("Désactivé");
            base.OnDisable();
        }

        protected internal override void OnEnable()
        {
            Log.Warning("Activé");
            base.OnEnable();
        }

        public Test() : base() { }
        public Test(string name) : base(name) { }
    }
}
