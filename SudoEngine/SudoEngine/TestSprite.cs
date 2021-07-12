using SudoEngine.Render;
using OpenTK.Input;

namespace SudoEngine
{
    public class TestSprite : Sprite
    {
        public TestSprite() : base("TestSprite") { }

        protected internal override void OnUpdate()
        {
            if (Keyboard.GetState().IsAnyKeyDown)
            {
                KeyboardState K = Keyboard.GetState();
                if (K.IsKeyDown(Key.Right)) RowInSpriteSheet = 1;
                if (K.IsKeyDown(Key.Left)) RowInSpriteSheet = 2;
            }
            else RowInSpriteSheet = 0;
            base.OnUpdate();
        }
    }
}
