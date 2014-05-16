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
    public class PlayerProjectile : Projectile
    {

        // fields used to track and draw animations
        Rectangle sourceRectangle;
        int currentFrame;
        const int FRAME_TIME = 10;
        int elapsedFrameTime = 0;

        const int NUM_FRAMES = 20;
        const int NUM_ROWS = 5;
        const int FRAMES_PER_ROW = 4;

        int WIN_WIDTH;
        int WIN_HEIGHT;

        public PlayerProjectile(ContentManager contentManager, string spriteName, int originX, int originY, int x, int y, int frameWidth, int frameHeight ) 
            : base(contentManager, spriteName, originX, originY)
        {
            
            velocity.X = x;
            velocity.Y = y;
            currentFrame = 0;
            frameWidth = sprite.Width/FRAMES_PER_ROW;
            frameHeight = sprite.Height/NUM_ROWS;
            LoadContent(originX, originY, frameWidth, frameHeight);

            WIN_WIDTH = frameWidth;
            WIN_HEIGHT = frameHeight;

        }

        public void Update(GameTime gameTime)
        {


            if (Active)
            {
                drawRectangle.X += (int)(velocity.X);
                drawRectangle.Y += (int)(velocity.Y);


            }

            // check for advancing animation frame


                if (Active)
                {
                    elapsedFrameTime += gameTime.ElapsedGameTime.Milliseconds;

                    if (elapsedFrameTime > FRAME_TIME)
                    {

                        elapsedFrameTime = 0;

                        // advance the animation
                        if (currentFrame < NUM_FRAMES)
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
            }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                spriteBatch.Draw(sprite, drawRectangle, sourceRectangle, Color.White);
            }
     
        }


       #region Private methods


        /// <summary>
        /// Loads the content for the explosion
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        public override void LoadContent(int originX, int originY, int frameWidth, int frameHeight)
        {

            // calculate frame size
            frameWidth = sprite.Width / FRAMES_PER_ROW;
            frameHeight = sprite.Height / NUM_ROWS;

            // set initial draw rectangle
            drawRectangle = new Rectangle(originX, originY, frameWidth, frameHeight);
            sourceRectangle = new Rectangle(0, 0, frameWidth, frameHeight);
        }

        /// <summary>
        /// Sets the source rectangle location to correspond with the given frame
        /// </summary>
        /// <param name="frameNumber">the frame number</param>
        private void SetSourceRectangleLocation(int frameNumber)
        {
            sourceRectangle.X = (frameNumber % FRAMES_PER_ROW) * frameWidth;
            sourceRectangle.Y = (frameNumber / FRAMES_PER_ROW) * frameHeight;
        }

        #endregion
 
    }
}

