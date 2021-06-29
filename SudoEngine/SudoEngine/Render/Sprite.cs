using System;
using SudoEngine.Core;
using System.Collections.Generic;

namespace SudoEngine.Render
{
    public class Sprite : GameObject
    {
        public static List<Sprite> AllSprites { get; set; } = new List<Sprite>();
        public Shader Shader { get; set; }

        public Texture SpriteSheet { get; private set; }

        public Sprite(string name = "BaseObject") : base(name) => AllSprites.Add(this);

        public override void Delete()
        {
            AllSprites.Remove(this);
            Shader.Delete();
            SpriteSheet.Delete();
            base.Delete();
        }
    }
}
