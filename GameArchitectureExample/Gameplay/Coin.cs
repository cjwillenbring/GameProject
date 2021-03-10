using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameArchitectureExample.Collisions;

namespace GameArchitectureExample.GamePlay
{
    public class Coin : FallingItem
    {
        protected const int Y_AXIS_ACCELERATION = 5;

        /// <summary>
        /// Timer holds animation time
        /// </summary>
        private double animationTimer;

        /// <summary>
        /// Holds the current animation frame
        /// </summary>
        private short animationFrame;

        /// <summary>
        /// Stores the location of the enemy object on the sprite atlas
        /// </summary>
        private static Rectangle atlas_location = new Rectangle(0, 0, 32, 132);

        public Coin() : base(2)
        {
            speed = new Vector2(0, rand.Next(30, 50));
            bounds = new BoundingRectangle((int) position.X, (int) position.Y, (int) 8, (int) 10);
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
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            // Every 3/10 of a second, advance the animation frame 
            if (animationTimer > .3)
            {
                animationFrame++;
                if (animationFrame > 3) animationFrame = 0;
                animationTimer = 0;
            }

            var sourceRect = new Rectangle(animationFrame * 32, 0, 32, 32);
            spriteBatch.Draw(atlas, position, sourceRect, Color.White, 0, new Vector2(11, 10), scalar, SpriteEffects.None, 0);
        }
    }
}
