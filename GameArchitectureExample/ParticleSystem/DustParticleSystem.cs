using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameArchitectureExample.ParticleSystem
{
    public class DustParticleSystem : ParticleSystem
    {

        public DustParticleSystem(Game game, int maxDust) : base(game, maxDust * 25)
        {

        }

        protected override void InitializeConstants()
        {
            textureFilename = "smoke";
            minNumParticles = 5;
            maxNumParticles = 10;

            blendState = BlendState.NonPremultiplied;
            DrawOrder = AdditiveBlendDrawOrder;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            var velocity = RandomHelper.NextDirection() * RandomHelper.NextFloat(20,80);

            var lifetime = RandomHelper.NextFloat(0.5f, 1.0f);

            var acceleration = -velocity / lifetime;

            var rotation = RandomHelper.NextFloat(0, MathHelper.TwoPi);

            var angularVelocity = RandomHelper.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4);

            p.Initialize(where, velocity, acceleration, lifetime: lifetime, rotation: rotation, angularVelocity: angularVelocity);
        }

        protected override void UpdateParticle(ref Particle particle, float dt)
        {
            base.UpdateParticle(ref particle, dt);

            float normalizedLiftime = particle.TimeSinceStart / particle.Lifetime;

            particle.Scale = 1;
        }

        public void PlaceDustCloud(Vector2 where)
        {
            AddParticles(where);
        }
    }
}
