using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using SudoEngine.Core;
using SudoEngine.Render;
using SudoEngine.Maths;
using System;
using System.Drawing;
using System.Collections.Generic;

namespace SudoEngine
{
    public class Window : GameWindow
    {
        public Window(int width, int height, string title) : base(width, height, GraphicsMode.Default, title, GameWindowFlags.Fullscreen, DisplayDevice.GetDisplay(DisplayIndex.Second)) { }

        readonly Camera camera = new Camera("Main");
        Shader shader;

        BackGround BG0;
        BackGround BG1;
        BackGround BG2;
        BackGround BG3;
        BackGround BG4;

        Texture texture0;
        Texture texture1;
        Texture texture2;
        Texture texture3;
        Texture texture4;

        Audio Audio = new Audio();
        Sound sound;

        protected override void OnLoad(EventArgs e)
        {
            //System.Windows.Forms.Cursor.Hide();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.Fog);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            shader = new Shader("Shader");

            shader.LoadFromFile("shaderTexture.vert", "shaderTexture.frag", null);
            camera.Shader = shader;
            texture0 = new Texture("TextureTest");
            texture0.LoadFromFile("sec6_caves.png", false);
            texture1 = new Texture("TextureTest");
            texture1.LoadFromFile("sec6_caves.png", false);
            texture2 = new Texture("TextureTest");
            texture2.LoadFromFile("sec6_caves.png", false);
            texture3 = new Texture("TextureTest");
            texture3.LoadFromFile("sec6_caves.png", false);
            texture4 = new Texture("TextureTest");
            texture4.LoadFromFile("sec6_caves.png", false);

            BackGround.CreateList();
            BG0 = new BackGround(Layer.BackGround, shader, texture0, new Vector2D(1, 1), "bgTest0");
            int[,] a = new int[,]
            {
                {140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140},
                {140, 140, 140, 141, 142, 152, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 140, 140, 140, 140, 140, 140},
                {140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140},
                {140, 140, 140, 141, 142, 152, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 0, 0, 160, 138, 139, 154, 155},
                {140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 0, 0, 176, 178, 99, 100, 101},
                {2, 9, 140, 141, 142, 152, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 0, 162, 164, 0, 12, 1},
                {18, 25, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 140, 0, 0, 0, 0, 0, 0, 0, 0, 28, 17},
                {34, 41, 140, 141, 142, 152, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 0, 0, 0, 0, 0, 0, 0, 0, 44, 33},
                {50, 57, 140, 141, 142, 152, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 141, 0, 0, 0, 0, 0, 0, 0, 0, 60, 49},
                {114, 115, 116, 117, 118, 119, 114, 115, 118, 119, 114, 115, 114, 115, 116, 117, 118, 119, 118, 119, 114, 115, 116, 117, 118, 119, 114, 115, 116, 117}
            };
            BG0.Generate(a, new Bitmap("Textures/0.png"));
            //BG1 = new BackGround(Layer.CloseBackGround, shader, texture1, new Vector2D(1, 1), "bgTest1");
            //BG2 = new BackGround(Layer.PlayerLayer, shader, texture2, new Vector2D(1, 1), "bgTest2");
            //BG3 = new BackGround(Layer.CloseForeGround, shader, texture3, new Vector2D(1, 1), "bgTest3");
            //BG4 = new BackGround(Layer.ForeGround, shader, texture4, new Vector2D(1, 1), "bgTest4");

            Audio.Init();
            
            sound = new Sound("test");
            sound.LoadFromFile("test3");
            IList<string> deviceList = Audio.DeviceList();
            for (int i = 0; i < deviceList.Count; i++) Log.Info(deviceList[i]);
            sound.Play();

            base.OnLoad(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Keyboard.GetState().IsAnyKeyDown)
            {
                if (Keyboard.GetState().IsKeyDown(Key.Keypad0) && BG0 != null) BG0.Visible = !BG0.Visible;
                if (Keyboard.GetState().IsKeyDown(Key.Keypad1) && BG1 != null) BG1.Visible = !BG1.Visible;
                if (Keyboard.GetState().IsKeyDown(Key.Keypad2) && BG2 != null) BG2.Visible = !BG2.Visible;
                if (Keyboard.GetState().IsKeyDown(Key.Keypad3) && BG3 != null) BG3.Visible = !BG3.Visible;
                if (Keyboard.GetState().IsKeyDown(Key.Keypad4) && BG4 != null) BG4.Visible = !BG4.Visible;

                shader.SetAttribute("MoveX", Keyboard.GetState().IsKeyDown(Key.Right));

                if (Keyboard.GetState().IsKeyDown(Key.ShiftLeft)) shader.SetAttribute("camera", true);
                if (Keyboard.GetState().IsKeyDown(Key.ShiftRight)) shader.SetAttribute("camera", false);

                if (Keyboard.GetState().IsKeyDown(Key.KeypadPlus)) WindowState = WindowState.Fullscreen;
                if (Keyboard.GetState().IsKeyDown(Key.KeypadMinus)) WindowState = WindowState.Normal;

                if (Keyboard.GetState().IsKeyDown(Key.Escape)) Exit();
                if (Keyboard.GetState().IsKeyDown(Key.AltLeft)) if (Keyboard.GetState().IsKeyDown(Key.F4)) Exit();
            }

            // Log.Info($"{(1.0D / e.Time):F0} FPS");

            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            camera.Use();

            BackGround.RenderAll();

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            Texture.DisposeAll();
            Shader.DisposeAll();
            BackGround.DisposeAll();
            Audio.Dispose();
            
            base.OnUnload(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }
    }
}
