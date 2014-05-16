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



    /// <summary>
    /// A Derived class for the Players Bear
    /// </summary>
    class PlayerBear : TeddyBear
    {

        int numHits = 3;


        // velocity information
        Vector2 velocity = new Vector2(0, 0);
        int x;
        int y;

        public List<PlayerProjectile> playerBullets = new List<PlayerProjectile>();
        int fireRate = 8;
        int lastShot = 0;

        public Shield shield;

        public PlayerBear(ContentManager contentManager, string spriteName, int windowWidth, int windowHeight) 
            : base( contentManager, spriteName, windowWidth, windowHeight)
        {
            shield = new Shield(contentManager, "FireShield", X, Y);

        }


        public void takeHit()
        {
            numHits -= 1;
            if (numHits == 0)
            {
                active = false;
            }
        }

        public void Heal()
        {
            numHits = 3;
        }

        public void Update(ContentManager content, MouseState mouse, PlayerState playerState, GameState state)
        {
            // move the teddy bear
            X = mouse.X;
            Y = mouse.Y;

            if (mouse.LeftButton == ButtonState.Pressed && state == GameState.Play)
            {
                Fire(content, playerState);

            }

            ShieldSelf(content, mouse, playerState);
        }

        #region Player Firing and Shield States

        private void ShieldSelf(ContentManager Content, MouseState mouse, PlayerState playerState)
        {
            shield.Update(mouse);
        }
        private void Fire(ContentManager Content, PlayerState playerState)
        {

            if (playerState == PlayerState.Normal)
            {
                if (lastShot == 0)
                {
                    playerBullets.Add(new PlayerProjectile(Content, "fireballs", drawRectangle.X + sprite.Width / 2, drawRectangle.Y, 0, -12, windowWidth, windowHeight));
                }

                lastShot++;

                if (lastShot > fireRate)
                {
                    lastShot = 0;
                }
            }
            if (playerState == PlayerState.DoubleShot)
            {
                if (lastShot == 0)
                {
                    playerBullets.Add(new PlayerProjectile(Content, "fireballs", drawRectangle.X - sprite.Width / 2, drawRectangle.Y, 0, -12, windowWidth, windowHeight));
                    playerBullets.Add(new PlayerProjectile(Content, "fireballs", drawRectangle.X + sprite.Width / 2, drawRectangle.Y, 0, -12, windowWidth, windowHeight));
                }

                lastShot++;

                if (lastShot > fireRate)
                {
                    lastShot = 0;
                }

            }
            if (playerState == PlayerState.MegaShot)
            {
                if (lastShot == 0)
                {
                    playerBullets.Add(new PlayerProjectile(Content, "fireballs", drawRectangle.X, drawRectangle.Y, 0, -12, windowWidth, windowHeight));
                    playerBullets.Add(new PlayerProjectile(Content, "fireballs", drawRectangle.X - sprite.Width / 2, drawRectangle.Y, -6, -12, windowWidth, windowHeight));
                    playerBullets.Add(new PlayerProjectile(Content, "fireballs", drawRectangle.X + sprite.Width / 2, drawRectangle.Y, 6, -12, windowWidth, windowHeight));
                }

                lastShot++;

                if (lastShot > fireRate)
                {
                    lastShot = 0;
                }
            }
        #endregion



            lastShot++;

            if (lastShot > fireRate)
            {
                lastShot = 0;
            }

        }

        /// <summary>
        /// Draws the teddy bear
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to use</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                spriteBatch.Draw(sprite, drawRectangle, Color.White);
            }
        }

        public int X
        {
            set
            {
                drawRectangle.X = value - drawRectangle.Width / 2;

                //Clamping code
                if (drawRectangle.Left < 0)
                {
                    drawRectangle.X = 0;
                }
                else if (drawRectangle.Right > windowWidth)
                {
                    drawRectangle.X = windowWidth - drawRectangle.Width;
                }
            }

            get { return x;  }
        }


        public int Y
        {
            set
            {
                drawRectangle.Y = value - drawRectangle.Height / 2;

                //Clamping code
                if (drawRectangle.Bottom > windowHeight)
                {
                    drawRectangle.Y = windowHeight - drawRectangle.Height;
                }
                else if (drawRectangle.Top < 0)
                {
                    drawRectangle.Y = 0 ;
                }
            }

            get { return y;  }
        }

    }
}
