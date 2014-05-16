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
    public class MenuButton
    {

        Texture2D play;

        Rectangle drawRectangle;

        GameState clickState;

        bool released;
        bool playSound;
 

        public MenuButton(ContentManager content, string spriteName, string spritename2, int x, int y, int scaler, GameState clickState)
        {
            this.clickState = clickState;

            LoadContent(content, spriteName, spritename2, x, y, scaler);
        }

        public void Update(MouseState mouse, SpriteBatch spriteBatch, ContentManager contentManager, SoundBank soundBank)
        {

            if (drawRectangle.Contains(mouse.X, mouse.Y))
            {
                play = contentManager.Load<Texture2D>("play_hl");

                if (playSound)
                { 
                    soundBank.PlayCue("menuToggle");
                    playSound = false;

                }


                if (mouse.LeftButton == ButtonState.Released )
                {
                    released = true;
                }

                if (mouse.LeftButton == ButtonState.Pressed && released == true)
                {
                     Game1.PlayMusic();
                }
            }
            else
            {
                play = contentManager.Load<Texture2D>("play");
                released = false;
                playSound = true;
            }
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(play, drawRectangle, Color.White);
        }


        private void LoadContent(ContentManager contentManager, string spriteName, string spriteName2, int x, int y, int scaler)
        {
            // load content and set remainder of draw rectangle
            play = contentManager.Load<Texture2D>(spriteName);
            drawRectangle = new Rectangle(x - play.Width / scaler, y - play.Height / scaler, play.Width, play.Height);
        }

    }
}
