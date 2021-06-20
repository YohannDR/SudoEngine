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
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.Fog);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            shader = new Shader("Shader");

            shader.LoadFromFile("shaderTexture.vert", "shaderTexture.frag", null);
            camera.Shader = shader;
            texture0 = new Texture("TextureTest");
            texture0.LoadFromFile("sec2_BG3.png", false);
            texture1 = new Texture("TextureTest");
            texture1.LoadFromFile("sec6_caves.png", false);
            texture2 = new Texture("TextureTest");
            texture2.LoadFromFile("sec6_caves.png", false);
            texture3 = new Texture("TextureTest");
            texture3.LoadFromFile("sec6_caves.png", false);
            texture4 = new Texture("TextureTest");
            texture4.LoadFromFile("sec6_caves.png", false);
            
            BackGround.CreateList();
            BG0 = new BackGround(Layer.BackGround, shader, texture0, new Vector2D(0.3, 0.2), "bgTest0");
            //BG1 = new BackGround(Layer.CloseBackGround, shader, texture1, new Vector2D(1, 1), "bgTest1");
            BG2 = new BackGround(Layer.PlayerLayer, shader, texture0, new Vector2D(1, 1), "bgTest0");
            int[,] a = new int[,]
            {
                {178, 100, 101, 102, 103, 98, 99, 102, 343, 138, 139, 140, 140, 140, 140},
                {194, 0, 0, 0, 0, 0, 0, 0, 359, 343, 156, 153, 140, 140, 140},
                {129, 0, 0, 0, 0, 0, 0, 0, 0, 359, 343, 156, 141, 140, 140},
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
                {0, 281, 280, 304, 305, 280, 0,  345, 309, 330, 0, 0, 0, 0, 0},
                {0, 279, 279, 0, 0, 279, 231, 232, 233, 346, 300, 0, 0, 0, 0},
                {0, 295, 280, 0, 0, 280, 0, 248, 0, 0, 252, 0, 0, 0, 0},
                {0, 0, 296, 0, 0, 281, 0, 248, 0, 173, 174, 175, 0, 0, 0},
                {0, 257, 258, 259, 0, 295, 262, 248, 0, 189, 190, 191, 0, 0, 0},
                {0, 273, 274, 275, 0, 245, 278, 264, 224, 205, 206, 207, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 240, 221, 222, 223, 0, 291, 277},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
            };
            //BG1.Generate(b, new Bitmap("Textures/0.png"));
            BG2.Generate(a, new Bitmap("Textures/0.png"));
            //BG3 = new BackGround(Layer.CloseForeGround, shader, texture3, new Vector2D(1, 1), "bgTest3");
            //BG4 = new BackGround(Layer.ForeGround, shader, texture4, new Vector2D(1, 1), "bgTest4");

            Audio.Init();
            
            sound = new Sound("test");
            sound.LoadFromFile("test3");
            IList<string> deviceList = Audio.DeviceList();
            //for (int i = 0; i < deviceList.Count; i++) Log.Info(deviceList[i]);
            //sound.Play();

            //foreach (DisplayIndex displayIndex in Enum.GetValues(typeof(DisplayIndex))) if (DisplayDevice.GetDisplay(displayIndex) != null && (int)displayIndex != -1) Log.Info($"Écran n°{(int)displayIndex} connecté");
            
            /*Log.Info($"A : {GamePad.GetCapabilities(1).HasAButton}");
            Log.Info($"B : {GamePad.GetCapabilities(1).HasBButton}");
            Log.Info($"X : {GamePad.GetCapabilities(1).HasXButton}");
            Log.Info($"Y : {GamePad.GetCapabilities(1).HasYButton}");
            Log.Info($"Back : {GamePad.GetCapabilities(1).HasBackButton}");
            Log.Info($"BigButton : {GamePad.GetCapabilities(1).HasBigButton}");
            Log.Info($"Dpad down : {GamePad.GetCapabilities(1).HasDPadDownButton}");
            Log.Info($"Dpad up : {GamePad.GetCapabilities(1).HasDPadUpButton}");
            Log.Info($"Dpad left : {GamePad.GetCapabilities(1).HasDPadLeftButton}");
            Log.Info($"Dpad right : {GamePad.GetCapabilities(1).HasDPadRightButton}");
            Log.Info($"Left shoulder : {GamePad.GetCapabilities(1).HasLeftShoulderButton}");
            Log.Info($"Left stick : {GamePad.GetCapabilities(1).HasLeftStickButton}");
            Log.Info($"Left trigger : {GamePad.GetCapabilities(1).HasLeftTrigger}");
            Log.Info($"Left vibration : {GamePad.GetCapabilities(1).HasLeftVibrationMotor}");
            Log.Info($"Left X thumb stick : {GamePad.GetCapabilities(1).HasLeftXThumbStick}");
            Log.Info($"Left Y thumb stick : {GamePad.GetCapabilities(1).HasLeftYThumbStick}");
            Log.Info($"Right shoulder : {GamePad.GetCapabilities(1).HasRightShoulderButton}");
            Log.Info($"Right stick : {GamePad.GetCapabilities(1).HasRightStickButton}");
            Log.Info($"Right trigger : {GamePad.GetCapabilities(1).HasRightTrigger}");
            Log.Info($"Right vibration : {GamePad.GetCapabilities(1).HasRightVibrationMotor}");
            Log.Info($"Right X thumb stick : {GamePad.GetCapabilities(1).HasRightXThumbStick}");
            Log.Info($"Right Y thumb stick : {GamePad.GetCapabilities(1).HasRightYThumbStick}");
            Log.Info($"Start : {GamePad.GetCapabilities(1).HasStartButton}");
            Log.Info($"Voice : {GamePad.GetCapabilities(1).HasVoiceSupport}");
            Log.Info($"Mapped : {GamePad.GetCapabilities(1).IsMapped}");
            Log.Info($"Connected : {GamePad.GetCapabilities(1).IsConnected}");*/

            GamePad.SetVibration(1, 1, 1);

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

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            
            if (e.Key == Key.Escape) Exit();
            if (e.Alt && e.Key == Key.F4) Exit();

            base.OnKeyDown(e);
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