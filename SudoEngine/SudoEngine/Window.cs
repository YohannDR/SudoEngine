using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using SudoEngine.Core;
using SudoEngine.Maths;
using SudoEngine.Render;
using System;
using System.Diagnostics;
using System.Drawing;

namespace SudoEngine
{
    public sealed class Window : GameWindow
    {
        public Window(int width, int height, string title) : base(width, height, GraphicsMode.Default, title, GameWindowFlags.Fullscreen, DisplayDevice.GetDisplay(DisplayIndex.Second)) { }

        readonly Camera camera = new Camera("Main");
        readonly Shader shader = new Shader();
        readonly Stopwatch SW = new Stopwatch();
        readonly BackGround BG0 = new BackGround();
        readonly BackGround BG1 = new BackGround();
        readonly BackGround BG2 = new BackGround();
        readonly Texture texture0 = new Texture();
        readonly Texture spriteSheet = new Texture();

        Sound sound;
        readonly Sprite sprite = new Sprite();

        Vector4D moveVector = new Vector4D(0);
        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            shader.LoadFromFile("shaderTexture.vert", "shaderTexture.frag", null);
            camera.Shader = shader;
            spriteSheet.LoadFromFile("spritesheet2.png");
            texture0.LoadFromFile("bg.png");

            sprite.Generate(spriteSheet, shader, 0, new Vector2D(64, 64));
            sprite.Position = new Vector2D(0, -325);

            BackGround.CreateList();
            int[,] a = new int[,]
            {
                {178, 100, 101, 102, 103, 98, 99, 102, 343, 138, 139, 140, 140, 140, 140},
                {194, 0, 0, 0, 0, 0, 0, 0, 359, 343, 156, 153, 140, 140, 140},
                {129, 0, 0, 0, 0, 0, 0, 0, 0, 359, 343, 159, 141, 140, 140},
                {145, 0, 0, 0, 0, 0, 0, 0, 0, 0, 359, 343, 157, 155, 140},
                {161, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 359, 180, 137, 140},
                {177, 0, 0, 0, 266, 0, 0, 0, 0, 0, 0, 0, 160, 152, 140},
                {97, 0, 0, 0, 282, 0, 0, 0, 0, 0, 0, 0, 176, 158, 136},
                {113, 0, 0, 0, 298, 0, 0, 0, 0, 0, 0, 0, 162, 98, 99},
                {210, 118, 119, 131, 116, 117, 114, 115, 105, 0, 0, 0, 0, 0, 0},
                {142, 152, 136, 138, 139, 153, 137, 143, 121, 118, 119, 114, 115, 116, 117}
            };

            int[,] b = new int[,]
            {
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 280, 281, 288, 289, 281, 290, 329, 293, 0, 0, 0, 0, 0, 0},
                {0, 281, 280, 304, 305, 280, 0, 345, 309, 330, 0, 0, 0, 0, 0},
                {0, 279, 279, 0, 0, 279, 231, 232, 233, 346, 300, 0, 0, 0, 0},
                {0, 295, 280, 0, 0, 280, 0, 248, 0, 0, 252, 0, 0, 0, 0},
                {0, 0, 296, 0, 0, 281, 0, 248, 0, 173, 174, 175, 0, 0, 0},
                {0, 257, 258, 259, 0, 295, 262, 248, 0, 189, 190, 191, 0, 0, 0},
                {0, 273, 274, 275, 0, 245, 278, 264, 224, 205, 206, 207, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 240, 221, 222, 223, 0, 291, 277},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
            };

            int[,] c = new int[,]
            {               
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {118, 119, 131, 116, 117, 114, 115, 118, 119, 131, 116, 117, 114, 115, 118, 119, 131, 116, 117, 114, 115, 118, 119, 131, 116, 117, 114, 115, 118, 119, 131, 116, 117, 114, 115, 118, 119, 131, 116, 117, 114, 115, 118, 119, 131, 116, 117, 114, 115, 118, 119, 131, 116, 117, 114, 115, 118, 119, 131, 116 },
            };

            BG2.Generate(Layer.PlayerLayer, shader, c, new Bitmap("Textures/1.png"));
            //BG1.Generate(Layer.CloseBackGround, shader, b, new Bitmap("Textures/1.png"));
            BG0.Generate(Layer.BackGround, shader, texture0, new Vector2D(1, 1));

            base.OnLoad(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Keyboard.GetState().IsKeyDown(Key.Right))
            {
                sprite.Position = new Vector2D(sprite.Position.X + 5, sprite.Position.Y);
                idx = 1;
            }
            if (Keyboard.GetState().IsKeyDown(Key.Left))
            {
                sprite.Position = new Vector2D(sprite.Position.X - 5, sprite.Position.Y);
                idx = 2;
            }
            if (!Keyboard.GetState().IsAnyKeyDown) idx = 0;

            sprite.DisplayImage(idx);
            GameObject.Update();
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            shader.Use();
            BackGround.RenderAll();
            GameObject.Render();

            Context.SwapBuffers();
            //Log.Info($"{(1.0D / e.Time):F0} FPS");
            base.OnRenderFrame(e);
        }
        int idx = 0;
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Keypad0 && BG0) BG0.Visible = !BG0.Visible;
            if (e.Key == Key.Keypad1 && BG1) BG1.Visible = !BG1.Visible;
            if (e.Key == Key.Keypad2 && BG2) BG2.Visible = !BG2.Visible;

            if (e.Key == Key.Escape) Exit();
            if (e.Alt && e.Key == Key.F4) Exit();

            /*if (e.Key == Key.Right) camera.Scroll(Direcction.Right, 0.05);
            if (e.Key == Key.Left) camera.Scroll(Direcction.Left, 0.05);
            if (e.Key == Key.Up) camera.Scroll(Direcction.Up, 0.05);
            if (e.Key == Key.Down) camera.Scroll(Direcction.Down, 0.055);
            if (e.Key == Key.Space) moveVector.W -= 0.01;
            if (e.Key == Key.BackSpace) moveVector.W += 0.01;*/
            base.OnKeyDown(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            Texture.DeleteAll();
            Shader.DeleteAll();
            BackGround.DeleteAll();
            Sound.DeleteAll();
            Audio.Delete();

            base.OnUnload(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }
    }
}