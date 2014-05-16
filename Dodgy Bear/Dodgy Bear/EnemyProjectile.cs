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
    public class EnemyProjectile : Projectile
    {

        int speed;
        

        /// <summary>
        /// Evil Teddy Bear Projectile constructor
        /// </summary>
        /// <param name="contentManager"></param>
        /// <param name="originX"></param>
        /// <param name="originY"></param>
        /// <param name="playerX"></param>
        /// <param name="playerY"></param>
        /// <param name="speedY"></param>
        public EnemyProjectile(ContentManager contentManager, string spriteName, int originX, int originY, int playerX, int playerY, int speed)
            : base(contentManager, spriteName, originX, originY)
        {
            this.speed = speed;

            frameWidth = sprite.Width;
            frameHeight = sprite.Height;
            LoadContent(originX, originY, frameWidth, frameHeight);

            // calculate frame size
            frameWidth = sprite.Width;
            frameHeight = sprite.Height;

            #region Targeting
            if ((playerX - originX) >= 0)
            {
                velocity.X = speed;
            }
                else if (playerX == originX)
                {
                    velocity.X = 0;
                }
                    else
                    {
                        velocity.X = (-1 * speed);
                    }
            

            if ( playerY > originY )
            {
                velocity.Y = speed;
            }
                else if(playerY == originY)
                {
                    velocity.Y = 0;
                }
                    else
                    {
                        velocity.Y = (-1 * speed);
                    }
            #endregion
        }

        public override void LoadContent(int originX, int originY, int frameWidth, int frameHeight)
        {
            // set initial draw rectangle
            drawRectangle = new Rectangle(originX + sprite.Width / 2, originY, frameWidth, frameHeight);
        }
    }
}
