using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DodgyBear
{
    public class Menu
    {
        MenuButton playButton;

        public Menu(ContentManager contentManager, int windowWidth, int windowHeight, GameState state)
        {
            playButton = new MenuButton(contentManager, "play", "play_hl", (windowWidth / 2), (windowHeight / 2), 2, state);
        }

        public void Update(MouseState mouseState, SpriteBatch spriteBatch, ContentManager contentManager, SoundBank soundBank)
        {
            playButton.Update(mouseState, spriteBatch, contentManager, soundBank);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            playButton.Draw(spriteBatch);
        }
    }
}
