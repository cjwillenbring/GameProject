using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameArchitectureExample.Collisions;

namespace GameArchitectureExample.GamePlay
{
    /// <summary>
    /// Abstract class to hold falling objects
    /// </summary>
    public abstract class FallingItem
    {
        /// <summary>
        /// Holds the viewportWidth of the screen
        /// </summary>
        protected static int viewportWidth;

        /// <summary>
        /// Stores the random generator for the falling item class
        /// </summary>
        protected static Random rand = new Random();

        ///<summary>
        /// The objects's position in the world
        ///</summary>
        protected Vector2 position;
        public Vector2 Position { get => position; }

        /// <summary>
        /// The objects's scale for when it gets rendered
        /// </summary>
        protected Vector2 scalar;
        public Vector2 Scalar { get => scalar; }

        /// <summary>
        /// The speed of the object falling
        /// </summary>
        protected Vector2 speed;

        /// <summary>
        /// The Bounding Shape
        /// </summary>
        protected ICollision bounds = null;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public ICollision Bounds { get => bounds; }

        /// <summary>
        /// Whether or not this item has been collided with
        /// </summary>
        public bool HasCollided { get; set; }

        public FallingItem(float scale)
        {
            float startingXPos = (float)rand.NextDouble();
            position = new Vector2(startingXPos* viewportWidth, 0);

            // deal with scaling
            scalar = new Vector2(scale, scale);

            // Make sure the bounds are set in the child class
            if (bounds == null) new Exception();
        }

        /// <summary>
        /// Register the viewport width
        /// </summary>
        /// <param name="w">The viewport width</param>
        public static void RegisterViewportWidth(int w)
        {
            viewportWidth = w;
        }


        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D atlas);
    }
}
