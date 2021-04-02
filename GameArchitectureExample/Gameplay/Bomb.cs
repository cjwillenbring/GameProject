using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameArchitectureExample.Collisions;
using GameArchitectureExample.ParticleSystem;

namespace GameArchitectureExample.GamePlay
{
    public class Bomb : FallingItem, IParticleEmitter
    {
        /// <summary>
        /// Holds the acceleration for the bomb object
        /// </summary>
        protected const int Y_AXIS_ACCELERATION = 10;

        /// <summary>
        /// Stores the location of the enemy object on the sprite atlas
        /// </summary>
        private static Rectangle atlas_location = new Rectangle(0, 145, 16, 16);

        public Vector2 Velocity => speed;

        public Vector2 EmmitterPosition => new Vector2(position.X - 15, position.Y - 15);

        public Bomb() : base((float)2.5)
        {
            speed = new Vector2(rand.Next(-20, 20), rand.Next(30, 50));
            bounds = new BoundingCircle((int)position.X, (int)position.Y, (int)(7 * scalar.X));
        }

        public override void Update(GameTime gameTime)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
            speed += t * Y_AXIS_ACCELERATION * Vector2.UnitY;
            position += speed * t;
            bounds.X = position.X;
            bounds.Y = position.Y;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D atlas)
        {
            spriteBatch.Draw(atlas, position, atlas_location, Color.White, 0, new Vector2(8, 10), scalar, SpriteEffects.None, 0);
        }
    }
}
