using OpenTK;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SudoEngine.Core
{
    public sealed class Sound : BaseObject
    {
        public static List<Sound> AllSounds { get; private set; } = new List<Sound>();

        public int Handle { get; private set; }
        public int Source { get; private set; }
        public int NumberChannels { get; private set; }
        public int SampleRate { get; private set; }
        public int BitsPerSample { get; private set; }
        public int Size { get; private set; }
        public ALFormat Format { get; private set; }

        public Sound(string name = "Sound") : base(name) => AllSounds.Add(this);

        void Generate(byte[] data)
        {
            Handle = AL.GenBuffer();
            Source = AL.GenSource();
            Format = NumberChannels == 1 ? BitsPerSample == 8 ? ALFormat.Mono8 : ALFormat.Mono16 : BitsPerSample == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;

            AL.BufferData(Handle, Format, data, data.Length, SampleRate);
            AL.Source(Source, ALSourcei.Buffer, Handle);
            AL.Source(Source, ALSourcef.Gain, 50);
        }

        public void Play() => AL.SourcePlay(Source);

        public void Pause() => AL.SourcePause(Source);

        public override void Delete()
        {
            AllSounds.Remove(this);
            AL.DeleteBuffer(Handle);
            AL.DeleteSource(Source);
            base.Delete();
        }

        public void LoadFromFile(string path)
        {
            if (!File.Exists("Sounds/" + path + ".wav"))
            {
                Log.Error($"Le fichier WAV n'existe pas : {path}");
                return;
            }

            //Pour plus d'infos sur la structure des fichiers WAV
            //https://docs.fileformat.com/audio/wav/

            byte[] buffer = File.ReadAllBytes("Sounds/" + path + ".wav");

            byte[] riffBytes = { buffer[0], buffer[1], buffer[2], buffer[3] };

            byte[] waveBytes = { buffer[8], buffer[9], buffer[10], buffer[11] };

            byte[] dataBytes = { buffer[36], buffer[37], buffer[38], buffer[39], };

            if (Encoding.ASCII.GetString(riffBytes) != "RIFF" || Encoding.ASCII.GetString(waveBytes) != "WAVE" || Encoding.ASCII.GetString(dataBytes) != "data")
            {
                Log.Error($"Le fichier n'est pas un WAV valide : {path}");
                return;
            }

            byte[] channelBytes = { buffer[22], buffer[23], 0, 0 };

            int numberChannels = BitConverter.ToInt32(channelBytes, 0);
            NumberChannels = numberChannels;

            byte[] sampleBytes = { buffer[24], buffer[25], buffer[26], buffer[27] };

            int sampleRate = BitConverter.ToInt32(sampleBytes, 0);
            SampleRate = sampleRate;

            byte[] bpsBytes = { buffer[34], buffer[35], 0, 0 };

            int bitsPerSample = BitConverter.ToInt32(bpsBytes, 0);
            BitsPerSample = bitsPerSample;

            byte[] sizeBytes = { buffer[40],  buffer[41], buffer[42], buffer[43] };

            Size = BitConverter.ToInt32(sizeBytes, 0); 

            byte[] data = new byte[Size];
            Array.Copy(buffer, 44, data, 0, Size);

            Generate(data);
        }

        public static void DeleteAll() { for (int i =0;i < AllSounds.Count; i++) if (AllSounds[i] != null) AllSounds[i].Delete(); }
    }

    public sealed class Music : BaseObject
    {
        public static List<Music> AllMusics { get; private set; } = new List<Music>();

        public int Handle { get; private set; }
        public int Source { get; private set; }
        public int SampleRate { get; private set; }
        public ALFormat Format { get; private set; }

        public Music(string name = "Music") : base(name) => AllMusics.Add(this);

        void Generate(byte[] data)
        {
            Handle = AL.GenBuffer();
            //Format
            AL.BufferData(Handle, Format, data, data.Length, SampleRate);
            Source = AL.GenSource();
            AL.Source(Source, ALSourcei.Buffer, Handle);
        }

        public void Play() => AL.SourcePlay(Source);
        public void Pause() => AL.SourcePause(Source);

        public override void Delete()
        {
            AllMusics.Remove(this);
            AL.DeleteBuffer(Handle);
            AL.DeleteSource(Source);
            base.Delete();
        }

        public void LoadFromFile(string path)
        {
            if (!File.Exists("Musics/" + path + ".mp3"))
            {
                Log.Error($"Le fichier MP3 n'existe pas : {path}");
                return;
            }

            byte[] buffer = File.ReadAllBytes("Musics/" + path + ".mp3");

            byte[] ideBytes =
            {
                buffer[0],
                buffer[1],
                buffer[2]
            };

            if (Encoding.ASCII.GetString(ideBytes) != "ID3")
            {
                Log.Error($"Le fichier n'est pas un MP3 valide : {path}");
                return;
            }


            byte[] data = new byte[1];
            Generate(data);
        }
    }

    public static class Audio
    {
        public static IntPtr Device { get; private set; }
        public static ContextHandle Context { get; private set; }

        public static void Init()
        {
            Device = Alc.OpenDevice(null);
            if (Device != null)
            {
                Context = Alc.CreateContext(Device, (int[])null);
                if (Context != null) Alc.MakeContextCurrent(Context);
                else Log.Error("Le contexte audio n'a pas pu être créé");
            }
            else Log.Error("Aucun device audio n'a pu être ouvert");
        }

        public static void Init(string deviceName)
        {
            Device = Alc.OpenDevice(deviceName);
            if (Device != null)
            {
                Context = Alc.CreateContext(Device, (int[])null);
                if (Context != null) Alc.MakeContextCurrent(Context);
                else Log.Error("Le contexte audio n'a pas pu être créé");
            }
            else Log.Error("Aucun device audio n'a pu être ouvert");
        }

        public static IList<string> DeviceList() => Alc.GetString((IntPtr)null, AlcGetStringList.AllDevicesSpecifier);

        public static void Delete()
        {
            Alc.MakeContextCurrent(ContextHandle.Zero);
            Alc.DestroyContext(Context);
            Alc.CloseDevice(Device);
        }
    }
}
