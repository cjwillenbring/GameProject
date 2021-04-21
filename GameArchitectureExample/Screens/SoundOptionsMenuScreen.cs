using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameArchitectureExample.Screens
{
    // The options screen is brought up over the top of the main menu
    // screen, and gives the user a chance to configure the game
    // in various hopefully useful ways.
    public class SoundOptionsMenuScreen : MenuScreen
    {
        private SpriteFont bangers;
        private SoundEffect coinPickupSound;

        private string _currentMasterVolumeMenuEntry;
        private string _currentMediaVolumeMenuEntry;

        private readonly MenuEntry _masterVolumeDownMenuEntry;
        private readonly MenuEntry _masterVolumeUpMenuEntry;
        private readonly MenuEntry _mediaPlayerVolumeDownMenuEntry;
        private readonly MenuEntry _mediaPlayerVolumeUpMenuEntry;

        private static float _currentMasterVolumeSetting;
        private static float _currentMediaVolumeSetting;


        public SoundOptionsMenuScreen() : base("")
        {
            _currentMasterVolumeSetting = SoundEffect.MasterVolume;
            _currentMediaVolumeSetting = MediaPlayer.Volume;

            _currentMasterVolumeMenuEntry = "";
            _currentMediaVolumeMenuEntry = "";

            _masterVolumeDownMenuEntry = new MenuEntry(string.Empty);
            _masterVolumeUpMenuEntry = new MenuEntry(string.Empty);
            _mediaPlayerVolumeDownMenuEntry = new MenuEntry(string.Empty);
            _mediaPlayerVolumeUpMenuEntry = new MenuEntry(string.Empty);
            SetMenuEntryText();

            var back = new MenuEntry("Back");

            _masterVolumeDownMenuEntry.Selected += MasterVolumeDownMenuEntrySelected;
            _mediaPlayerVolumeUpMenuEntry.Selected += MediaVolumeUpMenuEntrySelected;
            _mediaPlayerVolumeDownMenuEntry.Selected += MediaVolumeDownMenuEntrySelected;
            _masterVolumeUpMenuEntry.Selected += MasterVolumeUpMenuEntrySelected;

            back.Selected += OnCancel;

            MenuEntries.Add(_masterVolumeDownMenuEntry);
            MenuEntries.Add(_masterVolumeUpMenuEntry);
            MenuEntries.Add(_mediaPlayerVolumeDownMenuEntry);
            MenuEntries.Add(_mediaPlayerVolumeUpMenuEntry);

            _masterVolumeUpMenuEntry.Text = "Increase Sound Effect Volume";
            _masterVolumeDownMenuEntry.Text = "Decrease Sound Effect Volume";
            _mediaPlayerVolumeUpMenuEntry.Text = "Increase Music Volume";
            _mediaPlayerVolumeDownMenuEntry.Text = "Decrease Music Volume";

            MenuEntries.Add(back);
        }

        public override void Activate()
        {
            bangers = new ContentManager(ScreenManager.Game.Services, "Content").Load<SpriteFont>("bangers");
            coinPickupSound = new ContentManager(ScreenManager.Game.Services, "Content").Load<SoundEffect>("Pickup_Coin");

            base.Activate();
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            // Render text, measure widths first to get more precise placement
            var masterX = ScreenManager.GraphicsDevice.Viewport.Width / 2 - (bangers.MeasureString(_currentMasterVolumeMenuEntry).X / 2);
            var mediaX = ScreenManager.GraphicsDevice.Viewport.Width / 2 - (bangers.MeasureString(_currentMediaVolumeMenuEntry).X / 2);

            spriteBatch.DrawString(bangers, _currentMasterVolumeMenuEntry, new Vector2(masterX, 20), Color.Black, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(bangers, _currentMediaVolumeMenuEntry, new Vector2(mediaX, 60), Color.Black, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        // Fills in the latest values for the options screen menu text.
        private void SetMenuEntryText()
        {
            SoundEffect.MasterVolume = _currentMasterVolumeSetting;
            MediaPlayer.Volume = _currentMediaVolumeSetting;
            _currentMasterVolumeMenuEntry = $"Sound Effect Volume : {_currentMasterVolumeSetting.ToString("f2")}";
            _currentMediaVolumeMenuEntry = $"Music Volume : {_currentMediaVolumeSetting.ToString("f2")}";
        }

        private void MasterVolumeUpMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (_currentMasterVolumeSetting == 1) _currentMasterVolumeSetting = 1;
            else if (_currentMasterVolumeSetting + .05f >= 1) _currentMasterVolumeSetting = 1;
            else _currentMasterVolumeSetting += .05f;
            coinPickupSound.Play();
            SetMenuEntryText();
        }

        private void MediaVolumeUpMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (_currentMediaVolumeSetting == 1) _currentMediaVolumeSetting = 1;
            else if (_currentMediaVolumeSetting + .05f >= 1) _currentMediaVolumeSetting = 1;
            else _currentMediaVolumeSetting += .05f;

            SetMenuEntryText();
        }

        private void MasterVolumeDownMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (_currentMasterVolumeSetting == 0) _currentMasterVolumeSetting = 0;
            else if (_currentMasterVolumeSetting - .05f <= 0) _currentMasterVolumeSetting = 0;
            else _currentMasterVolumeSetting -= .05f;
            coinPickupSound.Play();
            SetMenuEntryText();
        }

        private void MediaVolumeDownMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (_currentMediaVolumeSetting == 0) _currentMediaVolumeSetting = 0;
            else if (_currentMediaVolumeSetting - .05f <= 0) _currentMediaVolumeSetting = 0;
            else _currentMediaVolumeSetting -= .05f;

            SetMenuEntryText();
        }
    }
}
