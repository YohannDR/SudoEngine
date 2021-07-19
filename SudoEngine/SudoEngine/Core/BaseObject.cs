using System;

namespace SudoEngine.Core
{
    /// <summary>Classe <see langword="abstract"/> qui fournit des fonctionnalités de base pour d'autres classes</summary>
    public abstract class BaseObject
    {
        /// <summary>Nom interne de l'objet</summary>
        protected internal string Name { get; private set; }

        /// <summary><see cref="Guid"/> servant d'ID interne de l'objet</summary>
        protected internal Guid ID { get; private set; }

        /// <summary><see cref="bool"/> indiquant si l'objet est activé ou non (<see langword="true"/> par défaut)</summary>
        protected internal bool Enabled { get; private set; } = true;

        /// <summary><see cref="bool"/> indiquant si l'objet est supprimé ou non (<see langword="false"/> par défaut)</summary>
        protected internal bool Deleted { get; private set; } = false;

        /// <summary>Crée un nouvel objet <see cref="BaseObject"/> et génère un <see cref="Guid"/></summary>
        protected internal BaseObject() => ID = Guid.NewGuid();

        /// <summary>
        /// Crée un nouvel objet <see cref="BaseObject"/> avec comme nom interne le nom spécifié et génère un <see cref="Guid"/>
        /// </summary>
        /// <param name="name">Le nom interne de l'objet</param>
        protected internal BaseObject(string name)
        {
            Name = name;
            ID = Guid.NewGuid();
        }

        /// <summary>Affecte <see langword="false"/> à <see cref="Deleted"/></summary>
        public virtual void Delete() => Deleted = true;

        /// <summary>
        /// Change l'état de l'objet
        /// </summary>
        /// <param name="status"><see cref="bool"/> indiquant le nouveau statut</param>
        public virtual void SetEnable(bool status) => Enabled = status;

        /// <summary>
        /// Converti en <see cref="string"/> l'objet
        /// </summary>
        /// <returns>Un string avec ce format : <see cref="Name"/> de type <see cref="Type"/></returns>
        public override string ToString() => $"{Name} de type {GetType().Name}";

        /// <summary>
        /// Vérifie si l'objet passé en paramètre est un <see cref="BaseObject"/>
        /// </summary>
        /// <param name="obj">L'objet à vérifier</param>
        /// <returns><see langword="true"/> si l'objet est un <see cref="BaseObject"/> sinon <see langword="false"/></returns>
        public override bool Equals(object obj) => obj is BaseObject;

        /// <summary>
        /// Génère le HashCode de l'<see cref="ID"/>
        /// </summary>
        /// <returns>Le HashCode de l'<see cref="ID"/></returns>
        public override int GetHashCode() => ID.GetHashCode();

        /// <summary>
        /// <see langword="true"/> si l'objet n'est pas <see langword="null"/> sinon <see langword="false"/>
        /// </summary>
        /// <param name="obj">L'objet à vérifier</param>
        public static implicit operator bool(BaseObject obj) => !(obj is null);
    }
}