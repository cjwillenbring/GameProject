using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameArchitectureExample.StateManagement;
using GameArchitectureExample.GamePlay;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using GameArchitectureExample.ParticleSystem;

namespace GameArchitectureExample.Screens
{
    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    public class GameplayScreen : GameScreen
    {
        private ContentManager _content;

        // Player Sprites
        private PlayerSprite player;
        private List<FallingItem> fallingItems;
        private ChestSprite chestSprite;

        // Platform Sprite
        private List<PlatformSprite> platforms;

        // Textures
        private Texture2D humble_atlas;
        private Texture2D colored_pack_atlas;
        private Texture2D ball;
        private Texture2D background_texture;
        private Texture2D coin;

        // Sound Effects and Music
        private SoundEffect coinPickupSound;
        private SoundEffect bombCoinPickupSound;
        private SoundEffect explosionSound;

        // Particle systems
        Dictionary<Bomb, PixieParticleSystem> _bombTrails = new Dictionary<Bomb, PixieParticleSystem>();
        CoinParticleSystem _coinSploshion;

        // Fonts
        private SpriteFont bangers;

        // Game properties/mechanics
        private int best;
        private int currentScore;
        private double countdownTimer;
        private double gameOverTimer;

        // Matrix transforms
        private bool _shaking;
        private float _shakeTime;
        private float _zoom;
        private float _zoomTime;
        private bool _zooming;

        private readonly Random random;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);

            // add in player and chest sprites
            player = new PlayerSprite();
            chestSprite = new ChestSprite();

            // Add countdown timer and reset score
            currentScore = 0;
            countdownTimer = 60;
            gameOverTimer = 0;

            // initialize the falling items list
            fallingItems = new List<FallingItem>() { };

            // initialize platform list and populate with static method
            platforms = new List<PlatformSprite>();
            PlatformBuilder.GeneratePlatforms(platforms);

            // transform
            _shaking = true;
            _shakeTime = 0;
            _zoom = 2f;
            _zoomTime = 0;
            _zooming = true;

            // initialize the random object
            random = new Random();
        }

        /// <summary>
        /// Reset the gameplay screen
        /// </summary>
        private void Reset()
        {
            countdownTimer = 60;
            currentScore = 0;
            fallingItems = new List<FallingItem>() { };
            foreach(Bomb b in _bombTrails.Keys)
            {
                ScreenManager.Game.Components.Remove(_bombTrails[b]);
            }
            _bombTrails = new Dictionary<Bomb, PixieParticleSystem>();
            gameOverTimer = 0;
            player.GameOver = false;
            player.Reset();
        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");


            // register the viewport width with the falling items class
            FallingItem.RegisterViewportWidth(ScreenManager.GraphicsDevice.Viewport.Width);

            // TODO: use this.Content to load your game content here
            // Loads player content, textures, etc
            player.LoadContent(_content);

            // Load textures
            chestSprite.LoadContent(_content);
            humble_atlas = _content.Load<Texture2D>("humble-item-pack");
            ball = _content.Load<Texture2D>("basketball");
            background_texture = _content.Load<Texture2D>("ground");
            coin = _content.Load<Texture2D>("coin-sparkle");
            colored_pack_atlas = _content.Load<Texture2D>("colored_packed");

            // Load Sound Effects and Music
            explosionSound = _content.Load<SoundEffect>("Explosion");
            coinPickupSound = _content.Load<SoundEffect>("Pickup_Coin");
            bombCoinPickupSound = _content.Load<SoundEffect>("Bomb_Coin");

            // Load fonts
            bangers = _content.Load<SpriteFont>("bangers");
            Reset();

            _zoom = 2f;
            _zooming = true;

            _coinSploshion = new CoinParticleSystem(ScreenManager.Game, 25);
            ScreenManager.Game.Components.Add(_coinSploshion);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                if (random.NextDouble() > .975 && gameOverTimer == 0)
                {
                    Bomb newBomb = new Bomb();
                    PixieParticleSystem newP = new PixieParticleSystem(ScreenManager.Game, newBomb);
                    ScreenManager.Game.Components.Add(newP);
                    _bombTrails.Add(newBomb, newP);
                    fallingItems.Add(newBomb);
                }
                if (random.NextDouble() > .975 && gameOverTimer == 0) fallingItems.Add(new Coin());
                double t = gameTime.ElapsedGameTime.TotalSeconds;
                if (gameOverTimer == 0) countdownTimer -= t;
                if (gameOverTimer > 0) gameOverTimer -= t;
                else if (gameOverTimer < 0) Reset();
                if (countdownTimer < 0) Reset();

                // TODO: Add your update logic here
                player.Update(gameTime, ScreenManager.GraphicsDevice.Viewport.Width, platforms);

                // Move through list of falling objects and get which ones have passed the bottom of the screen
                List<FallingItem> toRemove = new List<FallingItem>();
                foreach (var fallingItem in fallingItems)
                {
                    fallingItem.Update(gameTime);
                    if (fallingItem.Position.Y > ScreenManager.GraphicsDevice.Viewport.Height)
                    {
                        toRemove.Add(fallingItem);
                    }
                    best = Math.Max(best, currentScore);
                }

                // Set the player color to be white
                player.Color = Color.White;

                // Loop through falling items and look for collisions
                foreach (var item in fallingItems)
                {
                    if (item.Bounds.CollidesWith(player.Bounds))
                    {
                        player.Color = Color.Red;
                        item.HasCollided = true;
                        toRemove.Add(item);
                        // Add score logic here
                        if (item is Coin)
                        {
                            chestSprite.ChestState = ChestState.Open;
                            currentScore++;
                            coinPickupSound.Play(.2f, 0, 0);
                        }
                        else if (item is Bomb)
                        {
                            explosionSound.Play(.2f, 1, 0);
                            currentScore -= 5;
                            _coinSploshion.PlaceCoinSploshion(chestSprite.Position);
                            chestSprite.ChestState = ChestState.Open;
                            if (currentScore < 0 && gameOverTimer == 0)
                            {
                                _shaking = true;
                                _shakeTime = 0;
                                explosionSound.Play(.3f, 0, 0);
                                gameOverTimer = 1.2;
                                player.GameOver = true;
                                foreach (var f in fallingItems) toRemove.Add(f);
                            }
                        }
                    }
                }

                // Remove the items that have clipped through the bottom of the game
                foreach (var item in toRemove)
                {
                    fallingItems.Remove(item);
                    if (item is Bomb b)
                    {
                        if (_bombTrails.ContainsKey(b))
                        {
                            ScreenManager.Game.Components.Remove(_bombTrails[b]);
                            _bombTrails.Remove(b);
                        }
                    }
                }
            }
        }

        // Unlike the Update method, this will only be called when the gameplay screen is active.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Matrix zoomTranslation = Matrix.CreateTranslation(-640 / 2f, -480 / 2f, 0);
            if (_zooming)
            {
                _zoomTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                _zoom -= .05f;
                if (_zoom <= 1)
                {
                    _zooming = false;
                    _zoom = 1f;
                }
            }
            Matrix zoomScale = Matrix.CreateScale(_zoom);
            Matrix zoomTransform = zoomTranslation * zoomScale * Matrix.Invert(zoomTranslation);

            Matrix shakeTransform = Matrix.Identity;
            if (_shaking)
            {
                _shakeTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                shakeTransform = Matrix.CreateTranslation((20*_shakeTime / 1000) * MathF.Sin(_shakeTime), 10 * MathF.Cos(_shakeTime), 0);
                if (_shakeTime >= 1000) _shaking = false;
            }
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.DarkSlateGray, 0, 0);

            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;

            // TODO: Add your drawing code here
            spriteBatch.Begin(transformMatrix: shakeTransform * zoomTransform, samplerState: SamplerState.PointClamp);

            // Draw the background texture first since it should have the lowest z value and be rendered in the back
            spriteBatch.Draw(background_texture, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height), Color.White);
            
            foreach (var platform in platforms)
            {
                platform.Draw(gameTime, spriteBatch, colored_pack_atlas);
            }

            foreach (var item in fallingItems)
            {
                // think about making atlas more dynamic, but for now handle with if else
                if (item is Bomb b)
                    b.Draw(gameTime, spriteBatch, humble_atlas);
                else if (item is Coin c)
                    c.Draw(gameTime, spriteBatch, coin);
            }

            player.Draw(gameTime, spriteBatch);
            chestSprite.Draw(gameTime, spriteBatch);
            // Render text, measure widths first to get more precise placement
            Vector2 widthScore = bangers.MeasureString($"Current Score : {currentScore}");
            Vector2 widthBest = bangers.MeasureString($"Best : {best}");
            Vector2 widthPause = bangers.MeasureString($"Press Esc to Pause");
            spriteBatch.DrawString(bangers, $"Time Left : {countdownTimer:F}", new Vector2(5, 5), Color.Black);
            spriteBatch.DrawString(bangers, $"Current Score : {Math.Max(currentScore, 0)}", new Vector2(800 - (widthScore.X + 5), 5), Color.Black);
            spriteBatch.DrawString(bangers, $"Best : {best}", new Vector2(800 - (widthBest.X + 5), 45), Color.Black);
            spriteBatch.DrawString(bangers, $"Press Esc to Pause", new Vector2(800 - (widthPause.X + 5), 430), Color.White);
            spriteBatch.End();


            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
