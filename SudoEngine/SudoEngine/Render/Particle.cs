using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using SudoEngine.Core;
using SudoEngine.Maths;

namespace SudoEngine.Render
{
    public struct Particle
    {
        public Vector2D Position { get; set; }
        public Vector2D Velocity { get; set; }
        public Vector4D Color { get; set; }
        public double Rotation { get; set; }
        public double Age { get; set; }
        public double LifeTime { get; set; }

        public Particle(Vector2D position, Vector4D color, float lifeTime) => (Position, Velocity, Color, Rotation, Age, LifeTime) = (position, new Vector2D(0), color, 0, lifeTime, lifeTime);

        public static Particle NewBorn = new Particle(new Vector2D(0), new Vector4D(0), 0);
    }

    public sealed class ParticleEffect : GameObject
    {
        public static List<ParticleEffect> AllParticlesEffects = new List<ParticleEffect>();

        public List<Particle> Particles { get; private set; }
        public Texture GFX { get; set; }
        public Shader Shader { get; private set; }
        public int MaxParticles { get; set; }
        public double Speed { get; set; }
        public double SpeedOverLifeTime { get; set; }
        public Vector2D Size { get; set; }
        public double SizeOverLifeTime { get; set; }
        public int ParticlePerCycle { get; set; }
        int LastUsedParticle { get; set; }

        int VAO;

        public ParticleEffect(string name = "BaseObject") : base(name) => AllParticlesEffects.Add(this);

        public void Generate(Texture gfx, Shader shader, int maxParticles, float speed, float speedOverLifeTime, Vector2D size, float sizeOverLifeTime, int particlePerCycle)
        {
            GFX = gfx;
            Shader = shader;
            MaxParticles = maxParticles;
            Speed = speed;
            SpeedOverLifeTime = speedOverLifeTime;
            Size = size;
            SizeOverLifeTime = sizeOverLifeTime;
            ParticlePerCycle = particlePerCycle;
            for (int i = 0; i < maxParticles; i++) Particles.Add(Particle.NewBorn);
            VAO = GL.GenVertexArray();
        }

        public new void Update()
        {
            for (int i = 0; i < ParticlePerCycle; i++)
            {
                Particle p = Particles[FirstUnused()];
                unsafe
                {
                    Particle* Ppointer = &p;
                    NewParticle(Ppointer);
                }
            }

            for (int i = 0; i < MaxParticles; i++)
            {
                if (Particles[i].Age > 0)
                {
                    Particle p = Particles[i];
                    unsafe
                    {
                        Particle* Ppointer = &p;
                        UpdateParticle(Ppointer);
                    }
                }
            }
        }

        public new void Render()
        {
            GFX.Bind(TextureTarget.Texture2D);
            Shader.Use();
        }  

        int FirstUnused()
        {
            for (int i = LastUsedParticle; i < MaxParticles; i++)
            {
                if (Particles[i].Age <= 0)
                {
                    LastUsedParticle = i;
                    return i;
                }
            }
            LastUsedParticle = 0;
            return 0;
        }


        unsafe void UpdateParticle(Particle* p)
        {
            Particle P = *p;
            P.Age -= 0.1;
            P.Position += P.Velocity;
            *p = P;
        }

        unsafe void NewParticle(Particle* p)
        {
            
        }

        public override void Delete()
        {
            Shader.Delete();
            GFX.Delete();
            Particles.Clear();
            AllParticlesEffects.Remove(this);
            base.Delete();
        }
    }
}
