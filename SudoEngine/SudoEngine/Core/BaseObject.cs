using System;

namespace SudoEngine.Core
{
    public abstract class BaseObject
    {
        public string Name { get; private set; }
        public Guid ID { get; private set; }
        protected internal bool Enabled { get; set; } = true;
        protected internal bool Deleted { get; set; } = false;

        public BaseObject() => ID = Guid.NewGuid();

        public BaseObject(string name)
        {
            Name = name;
            ID = Guid.NewGuid();
        }

        public virtual void Delete() => Deleted = true;

        public virtual void SetEnable(bool status) => Enabled = status;

        public override string ToString() => $"{Name} de type {GetType().Name}";
        public override bool Equals(object obj) => obj is BaseObject;
        public override int GetHashCode() => ID.GetHashCode();

        public static implicit operator bool(BaseObject obj) => !(obj is null);
    }
}
