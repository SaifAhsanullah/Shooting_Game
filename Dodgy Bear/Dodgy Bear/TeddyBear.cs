using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DodgyBear
{
    /// <summary>
    /// A class for a teddy bear
    /// </summary>
    public class TeddyBear
    {
        #region Fields
        protected bool active = true;

        // drawing support
        protected Texture2D sprite;
        public Rectangle drawRectangle;

        // velocity information
        Vector2 velocity = new Vector2(0, 0);

        // bouncing support
        protected int windowWidth;
        protected int windowHeight;

        int respawnTimer;

        public List<EnemyProjectile> teddyBullets = new List<EnemyProjectile>();
        int lastShot = 600;
        int fireRate = RNG.getInt(125, 300);

        #endregion

        #region Constructors

        /// <summary>
        ///  Constructs a teddy bear with random direction and speed
        /// </summary>
        /// <param name="contentManager">the content manager for loading content</param>
        /// <param name="spriteName">the name of the sprite for the teddy bear</param>
        /// <param name="x">the x location of the center of the teddy bear</param>
        /// <param name="y">the y location of the center of the teddy bear</param>
        /// <param name="windowWidth">the window width</param>
        /// <param name="windowHeight">the window height</param>
        public TeddyBear(ContentManager contentManager, string spriteName, int x, int y, int windowWidth, int windowHeight)
        {
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            LoadContent(contentManager,spriteName, x, y);

            //generate random velocity
            int speed = RNG.getInt(3, 8);
            double angle = 5 * Math.PI * RNG.getDouble();
            velocity.X = (float)Math.Cos(angle) * speed;
            while (velocity.X == 0)
            {
                velocity.X = (float)Math.Cos(angle) * speed;
            }

            velocity.Y = -1 * (float)Math.Sin(angle) * speed;
            while (velocity.Y == 0)
            {
                velocity.Y = (float)Math.Sin(angle) * speed;
            }
        }

        public TeddyBear(ContentManager contentManager, string spriteName, int windowWidth, int windowHeight)
        {
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            LoadContent(contentManager, spriteName);

            //generate random velocity
            int speed = RNG.getInt(3, 8);
            double angle = 5 * Math.PI * RNG.getDouble();
            velocity.X = (float)Math.Cos(angle) * speed;
            while (velocity.X == 0)
            {
                velocity.X = (float)Math.Cos(angle) * speed;
            }

            velocity.Y = -1 * (float)Math.Sin(angle) * speed;
            while (velocity.Y == 0)
            {
                velocity.Y = (float)Math.Sin(angle) * speed;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and set whether or not the teddy bear is active
        /// </summary>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public void setLocation(int x, int y)
        {
            drawRectangle.X = x;
            drawRectangle.Y = y;
        }

        /// <summary>
        /// Gets the collision rectangle for the teddy bear
        /// </summary>
        public Rectangle CollisionRectangle
        {
            get { return drawRectangle; }
        }

        /// <summary>
        /// Used to set/reset bear location
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>

        #endregion

        #region Public methods

        /// <summary>
        /// Updates the teddy bear's location, bouncing if necessary
        /// </summary>
        public virtual void Update(ContentManager Content, int x, int y, SoundBank soundBank)
        {
            if (active)
            {
                // move the teddy bear
                drawRectangle.X += (int)(velocity.X);
                drawRectangle.Y += (int)(velocity.Y);

                // bounce as necessary
                BounceTopBottom();
                BounceLeftRight();

            }

            #region AI Firing
            if (lastShot == 0)
            {
                teddyBullets.Add(new EnemyProjectile(Content, "EnergyBall", drawRectangle.X + sprite.Width / 2, drawRectangle.Y, x, y, 7) );
                soundBank.PlayCue("evilFire");
            }

            lastShot++;

            if (lastShot > fireRate)
            {
                lastShot = 0;
            }

            #endregion

            if (respawnTimer >= 100 && active == false)
            {
                active = true;
                respawnTimer = 0;
                setLocation(RNG.getInt(0, windowWidth), RNG.getInt(0, windowHeight/2) );
            }

            respawnTimer++;
        }

        /// <summary>
        /// Draws the teddy bear
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to use</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                spriteBatch.Draw(sprite, drawRectangle, Color.Red);
            }
        }

        public void Bounce()
        {
            velocity.X *= -1;
            velocity.Y *= -1;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Loads the content for the teddy bear
        /// </summary>
        /// <param name="contentManager">the content manager to use</param>
        /// <param name="spriteName">the name of the sprite for the teddy bear</param>
        /// <param name="x">the x location of the center of the teddy bear</param>
        /// <param name="y">the y location of the center of the teddy bear</param>
        private void LoadContent(ContentManager contentManager, string spriteName, int x, int y)
        {
            // load content and set remainder of draw rectangle
            sprite = contentManager.Load<Texture2D>(spriteName);
            drawRectangle = new Rectangle(x - sprite.Width / 2,  y - sprite.Height / 2, sprite.Width, sprite.Height);
        }

        private void LoadContent(ContentManager contentManager, string spriteName)
        {
            // load content and set remainder of draw rectangle
            sprite = contentManager.Load<Texture2D>(spriteName);
            drawRectangle = new Rectangle(windowWidth / 2, windowHeight /2, sprite.Width, sprite.Height);
        }

        /// <summary>
        /// Bounces the teddy bear off the top and bottom window borders if necessary
        /// </summary>
        private void BounceTopBottom()
        {
            if (drawRectangle.Y < 0)
            {
                // bounce off top
                drawRectangle.Y = 0;
                velocity.Y *= -1;
            }
            else if ((drawRectangle.Y + drawRectangle.Height) > windowHeight)
            {
                // bounce off bottom
                drawRectangle.Y = windowHeight - drawRectangle.Height;
                velocity.Y *= -1;
            }
        }

        /// <summary>
        /// Bounces the teddy bear off the left and right window borders if necessary
        /// </summary>
        private void BounceLeftRight()
        {
            if (drawRectangle.X < 0)
            {
                // bounc off left
                drawRectangle.X = 0;
                velocity.X *= -1;
            }
            else if ((drawRectangle.X + drawRectangle.Width) > windowWidth)
            {
                // bounce off right
                drawRectangle.X = windowWidth - drawRectangle.Width;
                velocity.X *= -1;
            }
        }


        
        #endregion
    }
}
