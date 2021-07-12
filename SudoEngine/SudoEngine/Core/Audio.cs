using OpenTK;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SudoEngine.Core
{
    /// <summary>
    /// Classe qui représente un son, fourni un ensemble de méthodes et de propriétés qui facilitent la création et la manipulation
    /// <para>Hérite de <see cref="BaseObject"/> et ne peut pas être hérité</para>
    /// </summary>
    public sealed class Sound : BaseObject
    {
        /// <summary>Liste de tous les <see cref="Sound"/> chargés en mémoire</summary>
        public static List<Sound> AllSounds { get; private set; } = new List<Sound>();

        /// <summary>Handle du son (nécessaire au fonctionnement d'OpenAL)</summary>
        public int Handle { get; private set; }
        /// <summary>Handle de la source du son</summary>
        public int Source { get; private set; }
        /// <summary>Nombre de chaines du son (Mono ou Stereo)</summary>
        public int NumberChannels { get; private set; }
        /// <summary>Sample rate du son</summary>
        public int SampleRate { get; private set; }
        /// <summary>Bits par sample du son</summary>
        public int BitsPerSample { get; private set; }
        /// <summary>Taille des données du son</summary>
        public int Size { get; private set; }
        /// <summary><see cref="ALFormat"/> représentant le format du son</summary>
        public ALFormat Format { get; private set; }

        /// <summary>
        /// Crée un nouvel objet <see cref="Sound"/> et appele le constructeur de <see cref="BaseObject"/>
        /// </summary>
        /// <param name="name">Le nom interne de l'objet (Sound par défaut)</param>
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

        /// <summary>Joue le son</summary>
        public void Play() => AL.SourcePlay(Source);

        /// <summary>Met le son en pause</summary>
        public void Pause() => AL.SourcePause(Source);

        /// <summary>Supprime le son</summary>
        public override void Delete()
        {
            AllSounds.Remove(this);
            AL.DeleteBuffer(Handle);
            AL.DeleteSource(Source);
            base.Delete();
        }

        /// <summary>
        /// Crée un son à partir d'un fichier .WAV
        /// </summary>
        /// <param name="path">Le chemin vers le fichier du son (ajoute "Sounds/" devant et ".wav" derrière par défaut)</param>
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

        /// <summary>Supprime tous les <see cref="Sound"/></summary>
        public static void DeleteAll() { for (int i = 0; i < AllSounds.Count; i++) if (AllSounds[i] != null) AllSounds[i].Delete(); }
    }

    /// <summary>
    /// Classe qui représente une musique, fourni un ensemble de méthodes et de propriétés qui facilitent la création et la manipulation
    /// <para>Hérite de <see cref="BaseObject"/> et ne peut pas être hérité</para>
    /// <para>Pas encore implémenté</para>
    /// </summary>
    public abstract class Music : BaseObject
    {
        /// <summary>Liste de tous les <see cref="Music"/> chargés en mémoire</summary>
        public static List<Music> AllMusics { get; private set; } = new List<Music>();

        /// <summary>Handle de la musique (nécessaire au fonctionnement d'OpenAL)</summary>
        public int Handle { get; private set; }
        /// <summary>Handle de la source</summary>
        public int Source { get; private set; }
        /// <summary>Sample rate de la musique</summary>
        public int SampleRate { get; private set; }
        /// <summary><see cref="ALFormat"/> représentant le format de la musique</summary>
        public ALFormat Format { get; private set; }

        /// <summary>
        /// Crée un nouvel objet <see cref="Music"/> et appele le constructeur de <see cref="BaseObject"/>
        /// </summary>
        /// <param name="name">Le nom interne de l'objet (Music par défaut)</param>
        public Music(string name = "Music") : base(name) => AllMusics.Add(this);

        void Generate(byte[] data)
        {
            Handle = AL.GenBuffer();
            //Format
            AL.BufferData(Handle, Format, data, data.Length, SampleRate);
            Source = AL.GenSource();
            AL.Source(Source, ALSourcei.Buffer, Handle);
        }

        /// <summary>Joue le son</summary>
        public void Play() => AL.SourcePlay(Source);

        /// <summary>Met le son en pause</summary>
        public void Pause() => AL.SourcePause(Source);

        /// <summary>Supprime le son</summary>
        public override void Delete()
        {
            AllMusics.Remove(this);
            AL.DeleteBuffer(Handle);
            AL.DeleteSource(Source);
            base.Delete();
        }

        /// <summary>
        /// Crée une musique à partir d'un fichier .MP3
        /// </summary>
        /// <param name="path">Le chemin vers le fichier de la musique (ajoute "Musics/" devant et ".mp3" derrière par défaut)</param>
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

    /// <summary>Classe statique offrant des méthodes pour gérer OpenAL (contexte et device)</summary>
    public static class Audio
    {
        /// <summary>Le device actuellement ouvert</summary>
        public static IntPtr Device { get; private set; }
        /// <summary>Le contexte actuellement créé</summary>
        public static ContextHandle Context { get; private set; } 


        /// <summary>Crée un contexte OpenAL valide</summary>
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

        /// <summary>
        /// Crée un contexte OpenAL valide avec le device spécifié
        /// </summary>
        /// <param name="deviceName">Le nom du device</param>
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

        /// <summary>
        /// Récupère la liste des devices
        /// </summary>
        /// <returns><see cref="IList{String}"/> contenant la liste des noms des devices</returns>
        public static IList<string> DeviceList() => Alc.GetString((IntPtr)null, AlcGetStringList.AllDevicesSpecifier);
        
        /// <summary>Détruit le contexte et ferme le device</summary>
        public static void Delete()
        {
            Alc.MakeContextCurrent(ContextHandle.Zero);
            Alc.DestroyContext(Context);
            Alc.CloseDevice(Device);
        }
    }
}
