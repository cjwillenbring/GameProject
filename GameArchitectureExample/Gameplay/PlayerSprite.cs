using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameArchitectureExample.Collisions;
using Microsoft.Xna.Framework.Audio;

namespace GameArchitectureExample.GamePlay
{
    public enum Direction
    {
        Death = 4,
        Jump = 5,
        Fall = 6,
        Lateral = 1,
        Idle = 0
    }
    public class PlayerSprite
    {
        /// <summary>
        /// Timer holds animation time
        /// </summary>
        private double animationTimer;

        /// <summary>
        /// Holds the current animation frame
        /// </summary>
        private short animationFrame;

        /// <summary>
        /// Keyboard state
        /// </summary>
        private KeyboardState keyboardState;

        /// <summary>
        /// Player art/animations texture
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// Whether or not the sprite is facing to the left or right
        /// </summary>
        private bool flipped;

        /// <summary>
        /// Holds the player's speed
        /// </summary>
        private Vector2 speed = new Vector2(150, 150);

        /// <summary>
        /// Current position of the player
        /// </summary>
        private Vector2 position = new Vector2(300, 380);

        private BoundingRectangle bounds = new BoundingRectangle(300, 460, 38, 48);

        /// <summary>
        /// The bounding volume of the player
        /// </summary>
        public BoundingRectangle Bounds { get => bounds; }

        /// <summary>
        /// Holds the color of the player
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Get and set the current direction
        /// </summary>
        public Direction Direction { get; set; }

        /// <summary>
        /// Sets whether or not game is over or not
        /// </summary>
        public bool GameOver { get; set; }

        /// <summary>
        /// The lower bound for the player
        /// </summary>
        private const int lowerBound = 420;

        /// <summary>
        /// The upper boundary for the player
        /// </summary>
        private const int upperBound = 0;

        /// <summary>
        /// Whether or not the player is on a platform
        /// </summary>
        private bool OnPlatform = false;

        /// <summary>
        /// Holds the state of the last position
        /// </summary>
        private Vector2 lastPos;
        
        /// <summary>
        /// Holds the sound for the jump
        /// </summary>
        private SoundEffect jumpSound;

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("player");
            jumpSound = content.Load<SoundEffect>("Jump");
        }

        private bool IsInAir()
        {
            return position.Y < lowerBound - 48 || OnPlatform;
        }

        /// <summary>
        /// Updates the sprite's position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime, int screenWidth, List<PlatformSprite> platforms)
        {
            lastPos = position;
            float playerWidth = 40;
            float playerHeight = 48;
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 unitY = Vector2.UnitY;
            Vector2 unitX = Vector2.UnitX;
            Vector2 distanceTraveled = new Vector2(0, 0);
            keyboardState = Keyboard.GetState();

            // Handle keyboard left click
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                if(!IsInAir() || OnPlatform) Direction = Direction.Lateral;
                flipped = false;
                distanceTraveled += -unitX * speed * t;
            }
            // Handle keyboard right click
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                if (!IsInAir() || OnPlatform) Direction = Direction.Lateral;
                distanceTraveled += unitX * speed * t;
                flipped = true;
            }
            // Set direction to idle if player is not moving
            if (!(keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) && !((keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)))) Direction = Direction.Idle;

            // Allow player to clip through the wall
            if (position.X > screenWidth) position.X = 1;
            else if (position.X < 0) position.X = (float)(screenWidth - (playerWidth + 1));

            // Handle Gravity
            float gravity = 1000;
            if(!IsInAir()) speed.Y = 0;
            else speed.Y += gravity * t;

            // Apply instantaneous acceleration impulse if on ground and up key is pressed
            if ((keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) && (!IsInAir() || OnPlatform))
            {
                speed.Y += -30000 * t;
                jumpSound.Play(.2f, 0, 0);
            }
            distanceTraveled += unitY * speed * t;

            // Update direction based on speed if player is in the air
            if (IsInAir() && speed.Y > 0 && !OnPlatform) Direction = Direction.Fall;
            else if (IsInAir() && speed.Y <= 0) Direction = Direction.Jump;

            // Make sure player stays in bounds
            if (!IsInAir())
            {
                position.Y = (float)(lowerBound - playerHeight) + 1;
            }
            else if (position.Y < upperBound + playerHeight)
            {
                position.Y = (float)playerHeight + 1;
            }

            OnPlatform = false;
            // Handle platform logic
            if(position.Y >= 300 - playerHeight && speed.Y > 0 && (position.X > 334 - playerWidth && position.X < 472 + 46) && lastPos.Y <= 300 - playerHeight/2)
            {
                speed.Y = 0;
                distanceTraveled.Y = 0;
                OnPlatform = true;
            }
            if (position.Y >= 250 - playerHeight && speed.Y > 0 && (position.X > (334-275) - playerWidth && position.X < 472 - 275 + 46) && lastPos.Y <= 250 - playerHeight/2)
            {
                speed.Y = 0;
                distanceTraveled.Y = 0;
                OnPlatform = true;
            }
            if (position.Y >= 250 - playerHeight && speed.Y > 0 && (position.X > (334+250) - playerWidth && position.X < 472 + 250 + 46) && lastPos.Y <= 250 - playerHeight/2)
            {
                speed.Y = 0;
                distanceTraveled.Y = 0;
                OnPlatform = true;
            }

            position += distanceTraveled;
            if (GameOver) Direction = Direction.Death;
            bounds.X = position.X;
            bounds.Y = position.Y;
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            // Every 3/10 of a second, advance the animation frame 
            if (animationTimer > .3)
            {
                animationFrame++;
                if (animationFrame > 3) animationFrame = 0;
                animationTimer = 0;
            }

            var sourceRect = new Rectangle(animationFrame * 16, (int)Direction * 16, 16, 16);
            // Origin is calculated using the original size
            spriteBatch.Draw(texture, position, sourceRect, Color, 0, new Vector2(2, 0), 3, spriteEffects, 0);
        }
    }
}
