using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameArchitectureExample.StateManagement;

namespace GameArchitectureExample.Screens
{
    class HowToPlayScreen : MenuScreen
    {
        private string InstructionsText = "- You have 60 seconds to avoid the bombs \n    and collect as many coins as you can\n" +
                "- If you hit a bomb, your coins decrease by 5.\n- If your coins go below zero, you die and the game is over.\n" +
                "- Otherwise, the game will restart \n    upon reaching the 60 seconds\n" +
                "- To jump, press the up arrow or the W key\n" +
                "- To move laterally, press the side arrow keys of the D and A keys\n" +
                "- The game allows you to warp through the wall to the other side\n    Use this to your advantage to avoid the bombs\n" +
                "- Change the difficulty to change how often bombs spawn";

        private ContentManager _content;
        private SpriteFont bangers;


        public HowToPlayScreen() : base("")
        {
            var back = new MenuEntry("Back");
            back.Selected += OnCancel;
            back.Position = new Vector2(600, 40);
            MenuEntries.Add(back);
        }

        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            bangers = _content.Load<SpriteFont>("bangers");
            base.Activate();
        }


        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            // Render text, measure widths first to get more precise placement
            spriteBatch.DrawString(bangers, InstructionsText, new Vector2(10, 10), Color.Black, 0, new Vector2(0,0), .6f, SpriteEffects.None, 0);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        protected override void UpdateMenuEntryLocations()
        {
           
        }
    }
}
