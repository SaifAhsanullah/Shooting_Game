using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DodgyBear
{
    class Shield
    {
        int coolDown = 420;
        int duration = 120;
        int timer;
        public bool Active;

        Texture2D sprite;
        Rectangle drawRectangle;

        // hard-coded animation info.
        const int FRAMES_PER_ROW = 3;
        const int NUM_ROWS = 1;
        const int NUM_FRAMES = 3;

        // fields used to track and draw animations
        Rectangle sourceRectangle;
        int currentFrame;
        const int FRAME_TIME = 5;
        int elapsedFrameTime = 0;

        int frameWidth;
        int frameHeight;
        


        public Shield(ContentManager Content, string spriteName, int locationX, int locationY)
        {
            timer = 421;
            LoadContent(Content, spriteName, locationX, locationY);
        }


        public void LoadContent(ContentManager Content, string spriteName, int locationX, int locationY)
        {
            sprite = Content.Load<Texture2D>(spriteName);
            frameWidth = sprite.Width / FRAMES_PER_ROW;
            frameHeight = sprite.Height / NUM_ROWS;
            drawRectangle = new Rectangle(locationX, locationY, sprite.Width / 2, sprite.Height);
            sourceRectangle = new Rectangle(0, 0, frameWidth, frameHeight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                spriteBatch.Draw(sprite, drawRectangle, sourceRectangle, Color.White);
            }
        }



        public void Update(MouseState mouse)
        {
            timer++;
            elapsedFrameTime++;
            X = mouse.X;
            Y = mouse.Y;

            if (mouse.RightButton == ButtonState.Pressed && timer >= coolDown)
            {
                Active = true;
                timer = 0;
            }

            if (Active)
            {
                if (elapsedFrameTime > FRAME_TIME)
                { 
                
                    elapsedFrameTime = 0;

                    // advance the animation
                    if (currentFrame < NUM_FRAMES )
                    {
                        currentFrame++;
                        SetSourceRectangleLocation(currentFrame);
                    }
                    else if (currentFrame == NUM_FRAMES)
                    {
                        currentFrame = 0;
                        SetSourceRectangleLocation(currentFrame);
                    }
                }
            }


            if (timer > duration)
            {
                Active = false;

            }
        }
        

        public int X
        {
            set
            {
                drawRectangle.X = value - drawRectangle.Width / 2;
            }
        }

        public int Y
        {
            set
            {
                drawRectangle.Y = value - drawRectangle.Height / 2;
            }
        }


        /// <summary>
        /// Gets the collision rectangle for the teddy bear
        /// </summary>
        public Rectangle CollisionRectangle
        {
            get { return drawRectangle; }
        }

        private void SetSourceRectangleLocation(int frameNumber)
        {
            // calculate X and Y based on frame number
            sourceRectangle.X = (frameNumber % FRAMES_PER_ROW) * frameWidth;
            //sourceRectangle.Y = (frameNumber / FRAMES_PER_ROW) * frameHeight;
        }

    }
}
