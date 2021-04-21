using GameArchitectureExample.StateManagement;
using Microsoft.Xna.Framework;

namespace GameArchitectureExample.Screens
{
    // The pause menu comes up over the top of the game,
    // giving the player options to resume or quit.
    public class PauseMenuScreen : MenuScreen
    {
        public PauseMenuScreen() : base("Paused")
        {
            TitleColor = Color.White;
            var resumeGameMenuEntry = new MenuEntry("Resume Game");
            var quitGameMenuEntry = new MenuEntry("Quit To Main Menu");
            resumeGameMenuEntry.NonSelectedColor = Color.White;
            quitGameMenuEntry.NonSelectedColor = Color.White;

            resumeGameMenuEntry.Selected += OnCancel;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
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
