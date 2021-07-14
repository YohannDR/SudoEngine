using OpenTK.Input;
using SudoEngine.Core;
using SudoEngine.Render;

namespace SudoEngine
{
    public class TestSprite : Sprite
    {
        public TestSprite() : base("TestSprite") { }

        protected internal override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Right) RowInSpriteSheet = 1;
            if (e.Key == Key.Left) RowInSpriteSheet = 2;
        }

        protected internal override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            RowInSpriteSheet = 0;
        }
    }
}
