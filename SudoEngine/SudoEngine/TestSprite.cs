#if DEBUG
using SudoEngine.Render;
using SudoEngine.Maths;
using OpenTK.Input;

namespace SudoEngine
{
    public sealed class TestSprite : Sprite
    {
        public TestSprite() : base("TestSprite") { }

        public void Generate(Texture spriteSheet, Shader shader, double row, Vector2D size, Vector2D position)
        {
            Generate(spriteSheet, shader, row, size);
            Position = position;
        }

        protected internal override void OnUpdate()
        {
            KeyboardState K = Keyboard.GetState();
            if (K.IsAnyKeyDown)
            {
                if (K.IsKeyDown(Key.Right)) RowInSpriteSheet = 1;
                if (K.IsKeyDown(Key.Left)) RowInSpriteSheet = 2;
            }
            else RowInSpriteSheet = 0; ;
        }
    }
}
#endif