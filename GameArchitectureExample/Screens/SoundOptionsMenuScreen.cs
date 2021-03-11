

namespace GameArchitectureExample.Screens
{
    // The options screen is brought up over the top of the main menu
    // screen, and gives the user a chance to configure the game
    // in various hopefully useful ways.
    public class SoundOptionsMenuScreen : MenuScreen
    {
        private enum MasterVolumeSetting
        {
            Low,
            MediumLow,
            Medium,
            MediumHigh,
            High
        }

        private enum MediaPlayerSetting
        {
            Low,
            MediumLow,
            Medium,
            MediumHigh,
            High
        }

        private readonly MenuEntry _masterVolumeMenuEntry;
        private readonly MenuEntry _mediaPlayerVolumeMenuEntry;

        private static MasterVolumeSetting _currentMasterVolumeSetting = MasterVolumeSetting.Medium;
        private static MediaPlayerSetting _currentMediaVolumeSetting = MediaPlayerSetting.Medium;


        public SoundOptionsMenuScreen() : base("Options")
        {
            _masterVolumeMenuEntry = new MenuEntry(string.Empty);
            _mediaPlayerVolumeMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            var back = new MenuEntry("Back");

            _masterVolumeMenuEntry.Selected += MasterVolumeMenuEntrySelected;
            _mediaPlayerVolumeMenuEntry.Selected += MediaVolumeMenuEntrySelected;
            back.Selected += OnCancel;

            MenuEntries.Add(_masterVolumeMenuEntry);
            MenuEntries.Add(_mediaPlayerVolumeMenuEntry);
            MenuEntries.Add(back);
        }

        // Fills in the latest values for the options screen menu text.
        private void SetMenuEntryText()
        {
            _masterVolumeMenuEntry.Text = $"Master Volume : {_currentMasterVolumeSetting}";
            _mediaPlayerVolumeMenuEntry.Text = $"Media Volume : {_currentMediaVolumeSetting}";
        }

        private void MasterVolumeMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            _currentMasterVolumeSetting++;

            if (_currentMasterVolumeSetting > MasterVolumeSetting.High)
                _currentMasterVolumeSetting = 0;

            SetMenuEntryText();
        }

        private void MediaVolumeMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            _currentMediaVolumeSetting++;

            if (_currentMediaVolumeSetting > MediaPlayerSetting.High)
                _currentMediaVolumeSetting = 0;

            SetMenuEntryText();
        }
    }
}
