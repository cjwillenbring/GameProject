using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameArchitectureExample.ParticleSystem
{
    public class ExplosionParticleSystem : ParticleSystem
    {
        Rectangle _source;


        public ExplosionParticleSystem(Game game, int maxExplosions) : base(game, maxExplosions * 25)
        {

        }

        protected override void InitializeConstants()
        {
            textureFilename = "explosion";
            minNumParticles = 20;
            maxNumParticles = 25;

            blendState = BlendState.Additive;
            DrawOrder = AdditiveBlendDrawOrder;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            var velocity = RandomHelper.NextDirection() * RandomHelper.NextFloat(40,200);

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

            float alpha = 4 * normalizedLiftime * (1 - normalizedLiftime);
            particle.Color = Color.White * alpha;

            particle.Scale = .1f + .25f * normalizedLiftime;
        }

        public void PlaceExplosion(Vector2 where) => AddParticles(where);
    }
}
