using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace DodgyBear
{
    public enum GameState
    {
        MainMenu,
        GameOver,
        Play,
        Quit
    }

    public enum PlayerState
    {
        Normal,
        DoubleShot,
        MegaShot
    }


    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Audio Stuff
        public AudioEngine audioEngine;
        public WaveBank waveBank;
        static public SoundBank soundBank;
        static public Cue menuCue;
        static public Cue playCue;


        //Menu Stuff
        Menu mainMenu;
        GameOverMenu gameOverMenu;
        bool Reset = false;

        static public GameState state;
        static public PlayerState playerState;

        //Score Stuff
        Score score;

        const int WIN_WIDTH = 1280;
        const int WIN_HEIGHT = 768;

       
        //teddy Bears
        List<TeddyBear> Bears = new List<TeddyBear>();

        

        //Player Bear
        PlayerBear player;

        //Explosion
        List<Explosion> explosions = new List<Explosion>();
        int index = 0;

        //Number Generator


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //set resolution
            graphics.PreferredBackBufferWidth = WIN_WIDTH;
            graphics.PreferredBackBufferHeight = WIN_HEIGHT;
        }

        #region Change GameState and Music Cues
        /// <summary>
        /// Methods to change the GameState and audio
        /// </summary>
        /// <param name="clickState"></param>
        static public void PlayMusic()
        {
            state = GameState.Play;

            if (menuCue.IsPlaying)
            {
                menuCue.Pause();
            }

            if (playCue.IsPaused)
            {
                playCue.Resume(); 
            }
            else
            {
                playCue.Play();
            }
            
        }

        static public void MenuMusic()
        {

            if (playCue.IsPlaying)
            {
                playCue.Pause();
            }    

            if (menuCue.IsPaused)
            {
 
                menuCue.Resume();
            }
            else
            {
          
                menuCue.Play();
            }
                    
        }
        #endregion

        #region Change PlayerState
        public void ChangePlayerState()
        {

        }
        
        #endregion

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here



            base.Initialize();
            state = GameState.MainMenu;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            //Audio Stuff
            audioEngine = new AudioEngine(@"Content\GameAudio.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Sound Bank.xsb");
            menuCue = soundBank.GetCue("menuState");
            playCue = soundBank.GetCue("playState");
            MenuMusic();

            //Play Button
            mainMenu = new Menu(Content, WIN_WIDTH, WIN_HEIGHT, state);
            gameOverMenu = new GameOverMenu(Content, WIN_WIDTH, WIN_HEIGHT, state);

            #region Create Evil Teddy Bears
            for (int I = 0; I < 12; I++)
            {
                Bears.Add(new TeddyBear(Content, "teddybear1", RNG.getInt(0, WIN_WIDTH), RNG.getInt(0, WIN_HEIGHT/2), WIN_WIDTH, WIN_HEIGHT));
            }
            #endregion

            //Score Content to display score
            score = new Score(Content, "ScoreFont", WIN_WIDTH, WIN_HEIGHT);
            score.loadHighScores();
            playerState = PlayerState.Normal;

            #region Create Player Bear
            player = new PlayerBear(Content, "teddybear0", WIN_WIDTH, WIN_HEIGHT);
            #endregion
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();



            MouseState mouse = Mouse.GetState();
            player.Update(Content, mouse, playerState, state);

            if (state == GameState.MainMenu)
            {
                IsMouseVisible = true;
                mainMenu.Update(mouse, spriteBatch, Content, soundBank);
            }
            else if (state == GameState.GameOver)
            {
                IsMouseVisible = true;
                gameOverMenu.Update(mouse, spriteBatch, Content, soundBank);
            }
            else if (state == GameState.Play)
            {

                IsMouseVisible = false;

                //Reset Score from previous play
                if (Reset)
                {
                    score.Reset();
                    Reset = false;
                }
                score.Update();


                #region Player Firing Stuff

                if (score.playerScore >= 1000 && score.playerScore < 2000)
                {
                    playerState = PlayerState.DoubleShot;
                }
                else if (score.playerScore >= 2000)
                {
                    playerState = PlayerState.MegaShot;
                }

                for (int i = 0; i < player.playerBullets.Count; i++ )
                {
                    Rectangle rectangle = player.playerBullets[i].CollisionRectangle;

                    if (player.playerBullets[i].Active == true)
                    {
                        if (rectangle.X < 0 || rectangle.X > WIN_WIDTH)
                        {
                            player.playerBullets[i].Active = false;
                        }

                        if (rectangle.Y < 0 || rectangle.Y > WIN_HEIGHT)
                        {
                            player.playerBullets[i].Active = false;
                        }
                    }

                    if (player.playerBullets[i].Active == false)
                    {
                        player.playerBullets.Remove(player.playerBullets[i]);
                    }
                }
                #endregion




                //Update the Players bullets
                    foreach (PlayerProjectile bullet in player.playerBullets)
                    {
                        bullet.Update(gameTime);
                    }
                   
                //Evil Teddy Bear Updating
                    foreach (TeddyBear bear in Bears)
                    {
                        bear.Update(Content, player.drawRectangle.X, player.drawRectangle.Y, soundBank);
                    }

                    foreach (Explosion explosion in explosions)
                    {
                        explosion.Update(gameTime);
                    }









               #region Collision Detection

               //Check if player is hit by any enemy missiles or teddies, or if the player shield deflects it/them

               foreach (TeddyBear teddy in Bears)
               { 
                    foreach (EnemyProjectile bullet in teddy.teddyBullets)
                    {
                        if (bullet.Active)
                        {
                            bullet.Update();

                            if (player.shield.Active && player.shield.CollisionRectangle.Intersects(bullet.CollisionRectangle))
                            {
                                bullet.Active = false;
                            }

                            if (player.CollisionRectangle.Intersects(bullet.CollisionRectangle))
                            {
                                bullet.Active = false;
                                player.takeHit();
                            }
                        }
                    }
               }


               foreach (TeddyBear bear in Bears)
               {

                   foreach (PlayerProjectile bullet in player.playerBullets)
                   {                      
                       if (bear.Active && bullet.Active && bear.CollisionRectangle.Intersects(bullet.CollisionRectangle))
                       {
                           //Deactivate Dead Bears
                           bullet.Active = false;
                           bear.Active = false;

                           score.KillBonus();
                           soundBank.PlayCue("explosions");

                           //create a rectangle 
                           Rectangle ExplosionRectangle = Rectangle.Intersect(bear.CollisionRectangle, bullet.CollisionRectangle);
                           explosions.Add(new Explosion(Content));
                           explosions[index].Play(ExplosionRectangle.Center.X, ExplosionRectangle.Center.Y);
                           index++;
                       }
                   }
               }

               foreach (TeddyBear teddy in Bears)
               {
                    
                    if (player.shield.Active && teddy.Active && player.shield.CollisionRectangle.Intersects(teddy.CollisionRectangle))
                    {
                        teddy.Active = false;

                         score.KillBonus();
                         soundBank.PlayCue("explosions");

                          //create a rectangle 
                          Rectangle ExplosionRectangle = Rectangle.Intersect(teddy.CollisionRectangle, player.shield.CollisionRectangle);
                          explosions.Add(new Explosion(Content));
                          explosions[index].Play(ExplosionRectangle.Center.X, ExplosionRectangle.Center.Y);
                          index++;
                    }
                   
                   
                   if (teddy.Active && teddy.CollisionRectangle.Intersects(player.CollisionRectangle))
                    {
                        //Deactivate Dead Bears
                         teddy.Active = false;
                         player.takeHit();
                         score.KillBonus();
                         soundBank.PlayCue("explosions");


                         //Actually kill the Bears
                         Rectangle ExplosionRectangle = Rectangle.Intersect(teddy.CollisionRectangle, player.CollisionRectangle);
                         explosions.Add(new Explosion(Content));

                         explosions[index].Play(ExplosionRectangle.Center.X, ExplosionRectangle.Center.Y);
                         index++;
                    }                  
               }


               #endregion

////////////////////////////////// When Player Dies ///////////////////////////////////////////////////////

                    if (player.Active == false)
                    {
                        if (score.AddScore(score.playerScore) )
                        {
                            score.saveHighScores();
                        }
                        
                        state = GameState.GameOver;
                        playerState = PlayerState.Normal;
                        MenuMusic();
                        player.Active = true;
                        player.Heal();
                        explosions.Clear(); index = 0;
                        player.playerBullets.Clear();
                        Reset = true;


                        #region restore evil bears
                        foreach (TeddyBear teddy in Bears)
                        {
                            teddy.Active = true;
                            teddy.setLocation(RNG.getInt(0, WIN_WIDTH), RNG.getInt(0, WIN_HEIGHT / 2));
                            teddy.teddyBullets.Clear();
                        }


                        player.playerBullets.Clear();
                        #endregion


                    }
////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else
                {
                    this.Exit();
                }

                base.Update(gameTime);
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Draw Teddy Bears
            if (state == GameState.MainMenu )
            {
                spriteBatch.Begin();
                mainMenu.Draw(spriteBatch);
                spriteBatch.End();
            }

            if (state == GameState.GameOver)
            {
                spriteBatch.Begin();
                gameOverMenu.Draw(spriteBatch, score);
                spriteBatch.End();
            }

            if (state == GameState.Play)
            {
                spriteBatch.Begin();

                player.Draw(spriteBatch);
                score.Draw(spriteBatch);
                player.shield.Draw(spriteBatch);

                foreach (Explosion explosion in explosions)
                {
                    explosion.Draw(spriteBatch);
                }
                

                foreach (TeddyBear bear in Bears)
                {
                    bear.Draw(spriteBatch);
                }

                

                foreach (PlayerProjectile bullet in player.playerBullets)
                {
                    bullet.Draw(spriteBatch);
                }

                foreach (TeddyBear teddy in Bears)
                {
                    foreach (EnemyProjectile bullet in teddy.teddyBullets)
                    {
                        bullet.Draw(spriteBatch);
                    }
                }

                spriteBatch.End();
            }

            base.Draw(gameTime);
        }


    }
}


