using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameArchitectureExample.StateManagement;
using Microsoft.Xna.Framework.Media;

namespace GameArchitectureExample.Screens
{
    public class SplashScreen : GameScreen
    {
        ContentManager _content;
        Texture2D _background;
        TimeSpan _displayTime;
        SpriteFont bangers;
        private Song song;

        public override void Activate()
        {
            base.Activate();

            if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");
            _background = _content.Load<Texture2D>("splash_sky");
            bangers = _content.Load<SpriteFont>("bangers_large");
            _displayTime = TimeSpan.FromSeconds(2);
            song = _content.Load<Song>("Sabae-Sunrise");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);

            _displayTime -= gameTime.ElapsedGameTime;
            if (_displayTime <= TimeSpan.Zero) ExitScreen();
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(_background, Vector2.Zero, Color.White);
            ScreenManager.SpriteBatch.DrawString(bangers, $"COINS AND BOMBS", new Vector2(5, 100), Color.Black);
            ScreenManager.SpriteBatch.End();
        }
    }
}
