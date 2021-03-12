using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using GameArchitectureExample.StateManagement;
using GameArchitectureExample.GamePlay;

namespace GameArchitectureExample.Screens
{
    // The main menu screen is the first thing displayed when the game starts up.
    public class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen() : base("Main Menu")
        {
            var playGameMenuEntry = new MenuEntry("Play Game");
            var optionsMenuEntry = new MenuEntry("Sound Settings");
            var exitMenuEntry = new MenuEntry("Restart Game"); 
            var quitMenuEntry = new MenuEntry("Quit");

            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;
            quitMenuEntry.Selected += OnQuit;

            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
            MenuEntries.Add(quitMenuEntry);
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
                LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen());
            else
                LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, currentGameScreen);
        }

        private void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new SoundOptionsMenuScreen(), e.PlayerIndex);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to restart the game?";
            var confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

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
