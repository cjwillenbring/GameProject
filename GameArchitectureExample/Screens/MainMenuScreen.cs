using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using GameArchitectureExample.StateManagement;
using GameArchitectureExample.GamePlay;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameArchitectureExample.Screens
{
    // The main menu screen is the first thing displayed when the game starts up.
    public class MainMenuScreen : MenuScreen
    {
        private Difficulty difficultyProxy;
        public MainMenuScreen() : base("Coins and Bombs")
        {
            var playGameMenuEntry = new MenuEntry("Play Game");
            var changeDifficulty = new MenuEntry("Adjust Difficulty");
            var howToPlay = new MenuEntry("How to Play");
            var optionsMenuEntry = new MenuEntry("Sound Settings");
            var quitMenuEntry = new MenuEntry("Quit");

            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            howToPlay.Selected += HowToPlayMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            quitMenuEntry.Selected += OnQuit;
            changeDifficulty.Selected += ChangeDifficultyMenuEntrySelected;

            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(changeDifficulty);
            MenuEntries.Add(howToPlay);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(quitMenuEntry);
            difficultyProxy = GetCurrentDifficulty();
        }

        public override void Activate()
        {
            difficultyProxy = GetCurrentDifficulty();
            base.Activate();
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            var bangers = new ContentManager(ScreenManager.Game.Services, "Content").Load<SpriteFont>("bangers");
            // Render text, measure widths first to get more precise placement
            spriteBatch.DrawString(bangers, "Current Difficulty: " + difficultyProxy.ToString(), new Vector2(10, 10), Color.Black, 0, new Vector2(0, 0), .6f, SpriteEffects.None, 0);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        

        private void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            GameplayScreen currentGameScreen = null;
            foreach (var gs in ScreenManager.GetScreens())
            {
                if (gs is GameplayScreen g)
                {
                    currentGameScreen = g;
                    break;
                }
            }
            if (currentGameScreen == null)
                LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen(difficultyProxy));
            else
                LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, currentGameScreen);
        }

        private Difficulty GetCurrentDifficulty()
        {
            if (ScreenManager != null)
            {
                foreach (var gs in ScreenManager.GetScreens())
                {
                    if (gs is GameplayScreen g)
                    {
                        return g.Difficulty;
                    }
                }
            }
            return Difficulty.Easy;
        }

        private void ChangeDifficultyMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            foreach (var gs in ScreenManager.GetScreens())
            {
                if (gs is GameplayScreen g)
                {
                    switch(g.Difficulty)
                    {
                        case Difficulty.Easy:
                            g.Difficulty = Difficulty.Medium;
                            break;
                        case Difficulty.Medium:
                            g.Difficulty = Difficulty.Hard;
                            break;
                        case Difficulty.Hard:
                            g.Difficulty = Difficulty.Easy;
                            break;
                    }
                    difficultyProxy = g.Difficulty;
                    return;
                }
            }
            switch (difficultyProxy)
            {
                case Difficulty.Easy:
                    difficultyProxy = Difficulty.Medium;
                    break;
                case Difficulty.Medium:
                    difficultyProxy = Difficulty.Hard;
                    break;
                case Difficulty.Hard:
                    difficultyProxy = Difficulty.Easy;
                    break;
            }
        }

        private void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new SoundOptionsMenuScreen(), e.PlayerIndex);
        }

        private void HowToPlayMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new HowToPlayScreen(), e.PlayerIndex);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to quit the game?";
            var confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += OnQuitMessageAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        private void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            GameScreen[] gameScreens = ScreenManager.GetScreens();
            foreach(var screen in gameScreens)
            {
                ScreenManager.RemoveScreen(screen);
            }
            ScreenManager.AddScreen(new BackgroundScreen(), null);
            ScreenManager.AddScreen(new MainMenuScreen(), null);
            ScreenManager.AddScreen(new SplashScreen(), null);
            MediaPlayer.Volume = 1;
            SoundEffect.MasterVolume = 1;
        }

        private void OnQuit(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit the game?";
            var confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += OnQuitMessageAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, e.PlayerIndex);
        }

        private void OnQuitMessageAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}
