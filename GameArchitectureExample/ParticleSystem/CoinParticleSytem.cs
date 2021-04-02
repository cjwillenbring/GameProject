using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameArchitectureExample.ParticleSystem
{
    public class CoinParticleSystem : ParticleSystem
    {

        public CoinParticleSystem(Game game, int maxCoins) : base(game, maxCoins * 25)
        {

        }

        protected override void InitializeConstants()
        {
            textureFilename = "coin";
            minNumParticles = 15;
            maxNumParticles = 30;

            blendState = BlendState.NonPremultiplied;
            DrawOrder = AdditiveBlendDrawOrder;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            var velocity = RandomHelper.NextDirection() * RandomHelper.NextFloat(40,150);

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

            particle.Scale = 2;
        }

        public void PlaceCoinSploshion(Vector2 where)
        {
            AddParticles(where);
        }
    }
}
