using GameArchitectureExample.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameArchitectureExample.Screens
{
    // The pause menu comes up over the top of the game,
    // giving the player options to resume or quit.
    public class PauseMenuScreen : MenuScreen
    {
        private Difficulty difficultyProxy;

        public PauseMenuScreen() : base("Paused")
        {
            TitleColor = Color.White;
            var resumeGameMenuEntry = new MenuEntry("Resume Game");
            var changeDifficulty = new MenuEntry("Adjust Difficulty");
            var quitGameMenuEntry = new MenuEntry("Quit To Main Menu");
            resumeGameMenuEntry.NonSelectedColor = Color.White;
            changeDifficulty.NonSelectedColor = Color.White;
            quitGameMenuEntry.NonSelectedColor = Color.White;

            resumeGameMenuEntry.Selected += OnCancel;
            changeDifficulty.Selected += ChangeDifficultyMenuEntrySelected;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(changeDifficulty);
            MenuEntries.Add(quitGameMenuEntry);
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
            spriteBatch.DrawString(bangers, "Current Difficulty: " + difficultyProxy.ToString(), new Vector2(10, 10), Color.White, 0, new Vector2(0, 0), .6f, SpriteEffects.None, 0);
            spriteBatch.End();
            base.Draw(gameTime);
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
                    switch (g.Difficulty)
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

        private void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit?";
            var confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        // This uses the loading screen to transition from the game back to the main menu screen.
        private void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            // persist state across quits
            GameplayScreen currentGameScreen = null;
            foreach(var gs in ScreenManager.GetScreens())
            {
                if (gs is GameplayScreen g)
                {
                    currentGameScreen = g;
                    break;
                }
            }
            if(currentGameScreen == null)
                LoadingScreen.Load(ScreenManager, true, null, new BackgroundScreen(), new MainMenuScreen());
            else
                LoadingScreen.Load(ScreenManager, true, null, currentGameScreen, new BackgroundScreen(), new MainMenuScreen());
        }
    }
}
