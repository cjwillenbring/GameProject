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
    public class PlatformSprite
    {
        /// <summary>
        /// Current position of the player
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// The bounding box for the platform sprite
        /// </summary>
        private BoundingRectangle bounds;
        public BoundingRectangle Bounds { get => bounds; }
        
        /// <summary>
        /// holds the scale for the platforms
        /// </summary>
        private static int scale = 3;

        /// <summary>
        /// The location of the platform on the sprite atlas
        /// </summary>
        private static Rectangle atlas_location = new Rectangle(16*39, 16*15, 16, 16);

        /// <summary>
        /// Construct new platform sprite.
        /// </summary>
        /// <param name="pos">The position to place the platform</param>
        public PlatformSprite(Vector2 pos)
        {
            position = pos;
            bounds = new BoundingRectangle(position.X, position.Y, 16*scale, 16*scale);
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D atlas)
        {
            // Origin is calculated using the original size
            spriteBatch.Draw(atlas, position, atlas_location, Color.SaddleBrown, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0);
        }
    }
}
