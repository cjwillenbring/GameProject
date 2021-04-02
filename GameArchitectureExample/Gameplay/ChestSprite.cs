using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameArchitectureExample.Collisions;

namespace GameArchitectureExample.GamePlay
{
    public enum ChestState
    {
        Open = 1,
        Closed = 0
    }
    public class ChestSprite
    {
        /// <summary>
        /// Timer holds animation time
        /// </summary>
        private double animationTimer;

        /// <summary>
        /// Keyboard state
        /// </summary>
        public ChestState ChestState { get; set; }

        /// <summary>
        /// Current position of the player
        /// </summary>
        private Vector2 position = new Vector2(10, 420);

        public Vector2 Position => new Vector2(position.X + 20, position.Y + 20);

        /// <summary>
        /// Holds the color of the player
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Holds the texture for the content
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("chest-locked");
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(ChestState == ChestState.Open)
            {
                animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            }

            // Every 3/10 of a second, advance the animation frame 
            if (animationTimer > 1)
            {
                ChestState = ChestState.Closed;
                animationTimer = 0;
            }

            var sourceRect = new Rectangle((int)ChestState * 21, 0, 21, 18);
            // Origin is calculated using the original size
            spriteBatch.Draw(texture, position, sourceRect, Color, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);
        }
    }
}
