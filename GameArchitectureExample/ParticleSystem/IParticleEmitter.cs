using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameArchitectureExample.ParticleSystem
{
    public interface IParticleEmitter
    {
        public Vector2 EmmitterPosition { get; }

        public Vector2 Velocity { get; }
    }
}
