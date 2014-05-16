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

    public class Projectile
    {
        protected Rectangle drawRectangle;

        protected Texture2D sprite;

        protected int frameWidth;
        protected int frameHeight;

        protected Vector2 velocity;

        public bool Active;

        /// <summary>
        /// Projectile Construtcor
        /// </summary>
        /// <param name="contentManager"></param>
        /// <param name="spriteName"></param>
        /// <param name="originX"></param>
        /// <param name="originY"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Projectile(ContentManager contentManager, string spriteName, int originX, int originY) 
        {
            this.sprite = contentManager.Load<Texture2D>(spriteName);
            Active = true;
        }

        public virtual void Update()
        {
            if (Active)
            { 
                drawRectangle.X += (int)(velocity.X);
                drawRectangle.Y += (int)(velocity.Y);
            }
        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                spriteBatch.Draw(sprite, drawRectangle, Color.White);
            }
        }

        public virtual void LoadContent(int originX, int originY, int frameWidth, int frameHeight)
        {
           // set initial draw rectangle
            drawRectangle = new Rectangle(originX + sprite.Width / 2, originY, frameWidth, frameHeight);
        }

        public Rectangle CollisionRectangle
        {
            get { return drawRectangle; }
        }

    }
}
