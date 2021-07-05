using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using SudoEngine.Core;
using SudoEngine.Maths;
using SudoEngine.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

namespace SudoEngine
{
    public sealed class Window : GameWindow
    {
        public Window(int width, int height, string title) : base(width, height, GraphicsMode.Default, title, GameWindowFlags.Fullscreen, DisplayDevice.GetDisplay(DisplayIndex.Second)) { }

        readonly Camera camera = new Camera("Main");
        Shader shader = new Shader();
        Stopwatch a = new Stopwatch();

        BackGround BG0;
        BackGround BG1;
        BackGround BG2 = new BackGround();
        BackGround BG3;
        BackGround BG4;

        Texture texture0 = new Texture();
        Texture texture1;
        Texture texture2;
        Texture texture3;
        Texture texture4;

        Sound sound;
        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            shader.LoadFromFile("shaderTexture.vert", "shaderTexture.frag", null);

            texture0.LoadFromFile("testAtlas2.png");

            BackGround.CreateList();
            int[,] b = new int[,]
            {
                {0, 1, 2},
                {3, 4, 5},
                {6, 7, 8}
            };

            GameObject a = new GameObject("a");
            GameObject B = new GameObject("B");
            GameObject c = new GameObject("c");
            B.SetParent(c);
            a.SetParent(B);
            a.Hierarchy();
            BG2.Generate(Layer.PlayerLayer , shader, b, new Bitmap("Textures/TestAtlas2.png"));

            Audio.Init();
            sound = new Sound("test");
            sound.LoadFromFile("test3");
            //sound.Play();
            IList<string> deviceList = Audio.DeviceList();
            //foreach (string str in deviceList) Log.Info(str);
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
            Sprite s = new Sprite();
            //GamePad.SetVibration(1, 1, 1);
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
            }

            //Log.Info($"{(1.0D / e.Time):F0} FPS");

            GameObject.Update();
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            BackGround.RenderAll();
            GameObject.Render();

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if ((int)e.Key > 66 && (int)e.Key < 76) BG2.DeleteTile((int)e.Key - 67);
            if (e.Key == Key.Escape) Exit();
            if (e.Alt && e.Key == Key.F4) Exit();

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