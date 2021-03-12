using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace GameArchitectureExample.Screens
{
    // The options screen is brought up over the top of the main menu
    // screen, and gives the user a chance to configure the game
    // in various hopefully useful ways.
    public class SoundOptionsMenuScreen : MenuScreen
    {
        private readonly MenuEntry _currentMasterVolumeMenuEntry;
        private readonly MenuEntry _currentMediaVolumeMenuEntry;

        private readonly MenuEntry _masterVolumeDownMenuEntry;
        private readonly MenuEntry _masterVolumeUpMenuEntry;
        private readonly MenuEntry _mediaPlayerVolumeDownMenuEntry;
        private readonly MenuEntry _mediaPlayerVolumeUpMenuEntry;

        private static float _currentMasterVolumeSetting = SoundEffect.MasterVolume;
        private static float _currentMediaVolumeSetting = MediaPlayer.Volume;


        public SoundOptionsMenuScreen() : base("Options")
        {
            _currentMasterVolumeMenuEntry = new MenuEntry(string.Empty);
            _currentMediaVolumeMenuEntry = new MenuEntry(string.Empty);

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

            MenuEntries.Add(_currentMasterVolumeMenuEntry);
            MenuEntries.Add(_currentMediaVolumeMenuEntry);
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

        // Fills in the latest values for the options screen menu text.
        private void SetMenuEntryText()
        {
            SoundEffect.MasterVolume = _currentMasterVolumeSetting;
            MediaPlayer.Volume = _currentMediaVolumeSetting;
            _currentMasterVolumeMenuEntry.Text = $"Sound Effet Volume : {_currentMasterVolumeSetting.ToString("f2")}";
            _currentMediaVolumeMenuEntry.Text = $"Music Volume : {_currentMediaVolumeSetting.ToString("f2")}";
        }

        private void MasterVolumeUpMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (_currentMasterVolumeSetting == 1) _currentMasterVolumeSetting = 1;
            else if (_currentMasterVolumeSetting + .05f >= 1) _currentMasterVolumeSetting = 1;
            else _currentMasterVolumeSetting += .05f;

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
