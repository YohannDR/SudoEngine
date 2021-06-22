using System.Collections.Generic;
using SudoEngine.Core;
using SudoEngine.Maths;

namespace SudoEngine.Render
{
    struct Particle
    {
        public Vector2D Position { get; set; }
        public Vector2D Velocity { get; set; }
        public Vector4D Color { get; set; }
        public float Rotation { get; set; }
        public float Age { get; set; }
        public float LifeTime { get; set; }
    }

    class ParticleEffect : BaseObject
    {
        public static List<ParticleEffect> AllParticlesEffects = new List<ParticleEffect>();

        public List<Particle> Particles { get; private set; }
        public Texture GFX { get; set; }
        public Shader Shader { get; private set; }
        public int MaxParticles { get; set; }
        public float Speed { get; set; }
        public float SpeedOverLifeTime { get; set; }
        public Vector2D Size { get; set; }
        public float SizeOverLifeTime { get; set; }

        public ParticleEffect() : base() { AllParticlesEffects.Add(this); }
        public ParticleEffect(string name) : base(name) { AllParticlesEffects.Add(this); }

        public void Generate(Texture gfx, Shader shader, int maxParticles, float speed, float speedOverLifeTime, Vector2D size, float sizeOverLifeTime)
        {
            GFX = gfx;
            Shader = shader;
            MaxParticles = maxParticles;
            Speed = speed;
            SpeedOverLifeTime = speedOverLifeTime;
            Size = size;
            SizeOverLifeTime = sizeOverLifeTime;
        }

        public void Dispose()
        {
            Delete();
            Shader.Dispose();
            GFX.Dispose();
            Particles.Clear();
            AllParticlesEffects.Remove(this);
        }
    }
}
